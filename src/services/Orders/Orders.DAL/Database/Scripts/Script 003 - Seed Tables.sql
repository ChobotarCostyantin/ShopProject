BEGIN;

-- 1. Insert Customers
INSERT INTO customer (customer_id, full_name, email) VALUES
('11111111-1111-1111-1111-111111111111', 'Oleksandr Bondarenko', 'alex.bond@test.com'),
('22222222-2222-2222-2222-222222222222', 'Maria Kovalenko', 'maria.koval@test.com'),
('33333333-3333-3333-3333-333333333333', 'Ivan Petrov', 'ivan.petrov@test.com');

-- 2. Insert Orders
INSERT INTO order (order_id, customer_id, order_date, total_amount, status) VALUES
-- Order 1: Клієнт 1 (Oleksandr)
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '11111111-1111-1111-1111-111111111111', NOW() - INTERVAL '2 hours', 55000.00, 'New'),

-- Order 2: Клієнт 1 (Oleksandr)
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '11111111-1111-1111-1111-111111111111', NOW() - INTERVAL '20 days', 12000.00, 'Shipped'),

-- Order 3: Клієнт 2 (Maria)
('cccccccc-cccc-cccc-cccc-cccccccccccc', '22222222-2222-2222-2222-222222222222', NOW() - INTERVAL '1 day', 100000.00, 'Paid'),

-- Order 4: Клієнт 3 (Ivan)
('dddddddd-dddd-dddd-dddd-dddddddddddd', '33333333-3333-3333-3333-333333333333', NOW() - INTERVAL '5 hours', 28000.00, 'Cancelled');

-- 3. Insert Order Shipping
INSERT INTO order_shipping (shipping_id, order_id, address_line, city, postal_code) VALUES
-- Shipping for Order 1
('s1s1s1s1-s1s1-s1s1-s1s1-s1s1s1s1s1s1', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Khreshchatyk St, 1', 'Kyiv', '01001'),
-- Shipping for Order 2
('s2s2s2s2-s2s2-s2s2-s2s2-s2s2s2s2s2s2', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Khreshchatyk St, 1', 'Kyiv', '01001'),
-- Shipping for Order 3
('s3s3s3s3-s3s3-s3s3-s3s3-s3s3s3s3s3s3', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 'Soborna St, 15', 'Lviv', '79000'),
-- Shipping for Order 4
('s4s4s4s4-s4s4-s4s4-s4s4-s4s4s4s4s4s4', 'dddddddd-dddd-dddd-dddd-dddddddddddd', 'Deribasivska St, 10', 'Odesa', '65000');

-- 4. Insert Order Items
INSERT INTO order_item (order_item_id, order_id, product_id, product_name, unit_price, quantity) VALUES

-- Items for Order 1 (Total: 55,000) -> 1 MacBook Air
('i1i1i1i1-i1i1-i1i1-i1i1-i1i1i1i1i1i1', 
 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 
 'd1e1f104-d1e1-d1e1-d1e1-d1e1f1010004',
 'MacBook Air M3 13"',
 55000.00,
 1),

-- Items for Order 2 (Total: 12,000) -> 1 Sony Headphones
('i2i2i2i2-i2i2-i2i2-i2i2-i2i2i2i2i2i2', 
 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 
 'd1e1f106-d1e1-d1e1-d1e1-d1e1f1010006', 
 'Sony WH-1000XM5', 
 12000.00, 
 1),
('i3i3i3i3-i3i3-i3i3-i3i3-i3i3i3i3i3i3', 
 'cccccccc-cccc-cccc-cccc-cccccccccccc', 
 'd1e1f101-d1e1-d1e1-d1e1-d1e1f1010001', 
 'iPhone 15 Pro', 
 50000.00,
 2),

-- Items for Order 4 (Total: 28,000) -> 1 Google Pixel
('i4i4i4i4-i4i4-i4i4-i4i4-i4i4i4i4i4i4', 
 'dddddddd-dddd-dddd-dddd-dddddddddddd', 
 'd1e1f103-d1e1-d1e1-d1e1-d1e1f1010003', 
 'Google Pixel 8', 
 28000.00, 
 1);

COMMIT;