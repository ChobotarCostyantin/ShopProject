# **E-Commerce Shop – Microservices Design**

## **1️⃣ Orders Service (SQL + ADO.NET & Dapper)**

**Purpose:** Manage order processing, shipping details, and transaction history with **high performance procedural SQL**.

**Entities (4):**

1. **Customer** – `{customer_id (UUID), full_name, email, created_at}`
   * Acts as a local copy of user data for historical integrity.

2. **Order** – `{order_id (UUID), customer_id (FK), total_amount, status, order_date}`
   * Statuses: `New`, `Paid`, `Shipped`, `Cancelled`.

3. **OrderItem** – `{order_item_id (UUID), order_id (FK), product_id, product_name, unit_price, quantity}`
   * **Data Duplication:** Snapshots `product_name` and `unit_price` at the moment of purchase to prevent history corruption if catalog changes.
   * Logic: `Quantity > 0`.

4. **OrderShipping** – `{shipping_id (UUID), order_id (FK, Unique), address_line, city, postal_code}`
   * **One-to-One Relationship:** Each order has exactly one shipping record.
   * Constraint: `ON DELETE CASCADE`.

**Stored Procedures (CRUD & Logic):**
The service interacts with the database exclusively via 5 PostgreSQL procedures:
1. `sp_create_order` – Transactional insert of Order, Items, and Shipping.
2. `sp_get_order_summary` – Reads key order details via output parameters.
3. `sp_update_order_status` – Updates order state workflow.
4. `sp_update_shipping_address` – Updates address with logic check (cannot update if 'Shipped').
5. `sp_delete_order` – Performs cascade deletion of the order aggregate.

**SQL Features Highlighted:**
* **Strict Procedure Usage:** All operations use `CREATE PROCEDURE` (no functions).
* **INOUT Parameters:** Used for returning data from procedures in PostgreSQL.
* **Transaction Control:** Business logic encapsulated within database procedures.

---

## **2️⃣ Catalog Service (SQL + EF Core, Code First)**

**Purpose:** Store structured product data, categories, and technical specifications using **ORM capabilities**.

**Entities (5):**

1. **Category** – `{category_id (UUID), name}`

2. **Product** – `{product_id (UUID), category_id (FK), name, sku, price, stock_quantity}`
   * **Soft Delete:** `is_deleted` flag.
   * Unique Index: `sku` (Stock Keeping Unit).

3. **ProductDetail** – `{detail_id (UUID), product_id (FK, Unique), description, manufacturer, weight_kg}`
   * **One-to-One:** Separates heavy text data from the main product table.

4. **Tag** – `{tag_id (UUID), name}`

5. **ProductTag** – `{product_id (FK), tag_id (FK)}`
   * **Many-to-Many:** Connects Products and Tags.

**SQL Features Highlighted:**
* **EF Core Code First:** Fluent API configuration.
* **Complex Relationships:** 1:1 (Details), 1:N (Categories), M:N (Tags).
* **Constraints:** Check constraints for price, default values for stock.
---

## **3️⃣ Social & Reviews Service (MongoDB)**

**Purpose:** Handle high-volume user feedback, ratings, and community Q&A with **flexible schema**.

**Collections (3):**

1. **User** – `{_id, nickname, avatarUrl, reputationScore}`
   * Used as a reference for reviews.

2. **Review** – `{_id, productId, author, rating, text, comments[], createdAt}`
   * **Hybrid Pattern:** Stores `author.nickname` and `author.avatar` inside the review (snapshot) for fast reading, plus a `userId` reference.
   * **Embedded Documents:** `comments` are stored inside the review document.
   * **Denormalization:** Stores `productName` to avoid querying the Catalog service.

3. **CommunityPost** – `{_id, type, topic, ...}`
   * **Polymorphism (Schema Flexibility):**
     * *Type "Question":* `{text, isResolved, tags}`
     * *Type "Poll":* `{options: [{label, votes}], expiresAt}`

**MongoDB Features Highlighted:**
* **Embedding vs Referencing:** Comments are embedded; Users are referenced.
* **Polymorphism:** Different document structures in the same collection (`CommunityPost`).

---

### **Transactional Business Logic Examples**

1. **Create Order Transaction (Stored Procedure):**

```sql
BEGIN TRANSACTION
  1. Insert into Orders (Get new UUID)
  2. Insert into OrderItems (Loop through items, snapshot Price/Name)
  3. Insert into OrderShipping (Link to Order UUID)
COMMIT TRANSACTION
-- Rollback automatically on error
```

2. **Eventual Consistency (Cross-Service):**
    * When a user updates their Avatar in the User Service:
    * Event `UserAvatarUpdated` is published.
    * **Social Service** consumes event -> Updates `author.avatar` in recent Reviews (background job).

3. **Inventory Check:**
    * Before `sp_create_order` runs, the system validates stock availability via API call to Catalog Service.

---

### **Project Flow Overview**

1. **Catalog Service (SQL - EF Core)**
    * Admin manages products/categories. User browses catalog.

2. **Social Service (MongoDB)**
    * User checks reviews and Q&A before buying. Fast reads with no joins.

3. **Orders Service (SQL - ADO.NET & Dapper)**
    * User places an order.
    * A Stored Procedure ensures the Order, Items, and Shipping details are saved atomically.

---

### ✅ **Why This Design Works**

* **PostgreSQL (Orders):** Ensures money and shipping data never get lost or corrupted (ACID).
* **PostgreSQL (Catalog):** Strictly structured data is perfect for filtering products by category/tags/price.
* **MongoDB (Social):** Handles tree-like comment structures and variable content types (Polls vs Questions) effortlessly.
* **UUIDs:** Allows generating IDs on the client/API side, reducing database round-trips.

---

### **Service Ports**

<table>
  <tr>
    <th>Service</th>
    <th>Port</th>
    <th>Description</th>
  </tr>
  <tr>
    <td>Catalog Service</td>
    <td>5001</td>
    <td>Product Management API</td>
  </tr>
  <tr>
    <td>Orders Service</td>
    <td>5002</td>
    <td>Transactional API</td>
  </tr>
  <tr>
    <td>Social Service</td>
    <td>5003</td>
    <td>Reviews & Community API</td>
  </tr>
</table>