BEGIN;

-- 1. Insert Customers
INSERT INTO customers (customer_id, full_name, email) VALUES
('11111111-1111-1111-1111-111111111111', 'Oleksandr Bondarenko', 'alex.bond@test.com'),
('22222222-2222-2222-2222-222222222222', 'Maria Kovalenko', 'maria.koval@test.com'),
('33333333-3333-3333-3333-333333333333', 'Ivan Petrov', 'ivan.petrov@test.com');

-- 2. Insert Orders
INSERT INTO orders (order_id, customer_id, delivery_date, total_price, status, created_at) VALUES
-- Order 1: Клієнт 1 (Oleksandr)
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '11111111-1111-1111-1111-111111111111', NOW() + INTERVAL '1 day', 55000, 'New', NOW()),

-- Order 2: Клієнт 1 (Oleksandr)
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '11111111-1111-1111-1111-111111111111', NOW() - INTERVAL '4 days', 12000, 'Shipped', NOW() - INTERVAL '8 days'),

-- Order 3: Клієнт 2 (Maria)
('cccccccc-cccc-cccc-cccc-cccccccccccc', '22222222-2222-2222-2222-222222222222', NOW() + INTERVAL '10 days', 28000, 'Paid', NOW()),

-- Order 4: Клієнт 3 (Ivan)
('dddddddd-dddd-dddd-dddd-dddddddddddd', '33333333-3333-3333-3333-333333333333', NOW() + INTERVAL '3 days', 100000, 'Cancelled', NOW() - INTERVAL '3 days');

-- 3. Insert Order Shipping
INSERT INTO order_shipping (shipping_id, order_id, address_line, city, postal_code) VALUES
-- Shipping for Order 1
('a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Khreshchatyk St, 1', 'Kyiv', '01001'),
-- Shipping for Order 2
('b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Khreshchatyk St, 1', 'Kyiv', '01001'),
-- Shipping for Order 3
('c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 'Soborna St, 15', 'Lviv', '79000'),
-- Shipping for Order 4
('d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4', 'dddddddd-dddd-dddd-dddd-dddddddddddd', 'Deribasivska St, 10', 'Odesa', '65000');

-- 4. Insert Order Items
INSERT INTO order_items (order_item_id, order_id, product_id, quantity) VALUES

-- Items for Order 1 (Total: 55,000) -> 1 MacBook Air
('e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1', 
 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 
 'd1e1f104-d1e1-d1e1-d1e1-d1e1f1010004',
 1),
-- Items for Order 2 (Total: 12,000) -> 1 Sony Headphones
('f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2', 
 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 
 'd1e1f106-d1e1-d1e1-d1e1-d1e1f1010006', 
 1),

-- Items for Order 3 (Total: 100000) -> 2 Sony MX400
('03030303-0303-0303-0303-030303030303', 
 'cccccccc-cccc-cccc-cccc-cccccccccccc', 
 'd1e1f101-d1e1-d1e1-d1e1-d1e1f1010001', 
 2),

-- Items for Order 4 (Total: 28,000) -> 1 Google Pixel
('14141414-1414-1414-1414-141414141414', 
 'dddddddd-dddd-dddd-dddd-dddddddddddd', 
 'd1e1f103-d1e1-d1e1-d1e1-d1e1f1010003', 
 1);

COMMIT;