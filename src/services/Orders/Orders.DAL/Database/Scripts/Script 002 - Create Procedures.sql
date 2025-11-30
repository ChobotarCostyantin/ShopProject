-- 1. CREATE: Транзакційне створення замовлення
CREATE OR REPLACE PROCEDURE create_order(
    IN P_order_id UUID,
    IN p_customer_id UUID,
    IN p_delivery_date TIMESTAMP,
    IN p_total_price DECIMAL,
    IN p_status VARCHAR,
    IN p_created_at TIMESTAMP
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO order (order_id, customer_id, delivery_date, total_price, status, created_at)
    VALUES (p_order_id, p_customer_id, p_delivery_date, p_total_price, p_status, p_created_at);
END;
$$;

-- 2. CREATE: Транзакційне створення клієнта
CREATE OR REPLACE PROCEDURE create_customer(
    IN p_customer_id UUID,
    IN p_full_name VARCHAR,
    IN p_email VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO customer (customer_id, full_name, email)
    VALUES (p_customer_id, p_full_name, p_email);
END;
$$;

-- 3. CREATE: Транзакційне створення предмету замовлення
CREATE OR REPLACE PROCEDURE create_order_item(
    IN p_order_item_id UUID,
    IN p_order_id UUID,
    IN p_product_id UUID,
    IN p_quantity INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO order_item (order_item_id, order_id, product_id, quantity)
    VALUES (p_order_item_id, p_order_id, p_product_id, p_quantity);
END;
$$;

-- 4. CREATE: Транзакційне створення доставки замовлення
CREATE OR REPLACE PROCEDURE create_order_shipping(
    IN p_shipping_id UUID
    IN p_order_id UUID,
    IN p_address_line VARCHAR,
    IN p_city VARCHAR,
    IN p_postal_code VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO order_shipping (shipping_id, order_id, address_line, city, postal_code)
    VALUES (p_shipping_id, p_order_id, p_address_line, p_city, p_postal_code);
END;
$$;
