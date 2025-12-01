CREATE TABLE IF NOT EXISTS customers (
    customer_id UUID PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS orders (
    order_id UUID PRIMARY KEY,
    customer_id UUID NOT NULL,
    delivery_date TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(),
    total_price DECIMAL(18, 2) NOT NULL,
    status VARCHAR(50) DEFAULT 'New',
    created_at TIMESTAMP DEFAULT NOW(),
    CONSTRAINT fk_order_customers FOREIGN KEY (customer_id) REFERENCES customers(customer_id) ON DELETE CASCADE,
    CONSTRAINT ck_total_price CHECK (total_price >= 0)
);

CREATE TABLE IF NOT EXISTS order_shipping (
    shipping_id UUID PRIMARY KEY,
    order_id UUID UNIQUE NOT NULL,
    address_line VARCHAR(200) NOT NULL,
    city VARCHAR(50) NOT NULL,
    postal_code VARCHAR(20),
    CONSTRAINT fk_shipping_order FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS order_items (
    order_item_id UUID PRIMARY KEY,
    order_id UUID NOT NULL,
    product_id UUID NOT NULL,
    quantity INT NOT NULL,
    CONSTRAINT fk_items_order FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE,
    CONSTRAINT ck_quantity CHECK (quantity > 0)
);

CREATE INDEX IF NOT EXISTS ix_order_customer_id ON orders(customer_id);
CREATE INDEX IF NOT EXISTS ix_order_item_order_id ON order_items(order_id);
