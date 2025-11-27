CREATE TABLE customers (
    customer_id UUID PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE orders (
    order_id UUID PRIMARY KEY,
    customer_id UUID NOT NULL,
    order_date TIMESTAMP DEFAULT NOW(),
    total_price DECIMAL(18, 2) NOT NULL
    status VARCHAR(50) DEFAULT 'New',
    created_at TIMESTAMP DEFAULT NOW(),
    CONSTRAINT fk_order_customers FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
    CONSTRAINT ck_total_amount CHECK (total_amount >= 0)
);

CREATE TABLE order_shipping (
    shipping_id UUID PRIMARY KEY,
    order_id UUID UNIQUE NOT NULL,
    address_line VARCHAR(200) NOT NULL,
    city VARCHAR(50) NOT NULL,
    postal_code VARCHAR(20),
    CONSTRAINT fk_shipping_order FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE
);

CREATE TABLE order_items (
    order_item_id UUID PRIMARY KEY,
    order_id UUID NOT NULL,
    product_id UUID NOT NULL,
    product_name VARCHAR(150) NOT NULL,
    unit_price DECIMAL(18, 2) NOT NULL,
    quantity INT NOT NULL
    CONSTRAINT fk_items_order FOREIGN KEY (order_id) REFERENCES orders(order_id)
    CONSTRAINT ck_quantity CHECK (quantity > 0)
);

CREATE INDEX ix_order_customer_id ON orders(customer_id);
CREATE INDEX ix_order_item_order_id ON order_items(order_id);
