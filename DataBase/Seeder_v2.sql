-- Insert into Categories Table
INSERT INTO Categories (CategoryName)
VALUES 
('Chinese'),
('Dessert'),
('Fast Food'),
('Italian'),
('Japanese'),
('Snack'),
('Spicy'),
('Vegan');

-- Seed data for Restaurants
INSERT INTO Restaurants (Email, Name, PhoneNumber, Address, City, Country, CreatedAt)
VALUES 
('info@lyongourmet.fr', 'Lyon Gourmet', '+33478900123', '12 Rue des Gourmands, 69001', 'Lyon', 'France', GETDATE()),
('contact@bistrodeluxe.fr', 'Bistro Deluxe', '+33478900234', '25 Avenue des Saveurs, 69002', 'Lyon', 'France', GETDATE()),
('reservations@lyonclassique.fr', 'Lyon Classique', '+33478900345', '89 Place Bellecour, 69002', 'Lyon', 'France', GETDATE()),
('info@veganlyon.fr', 'Vegan Lyon', '+33478900456', '37 Rue des Verts, 69003', 'Lyon', 'France', GETDATE()),
('hello@pizzalyon.fr', 'Pizza Lyon', '+33478900567', '5 Rue de la Pizza, 69004', 'Lyon', 'France', GETDATE());

-- Seed data for Menus
INSERT INTO Menus (Name, CreatedAt, Id_Restaurants)
VALUES 
('Lunch Specials', GETDATE(), 1),
('Dinner Specials', GETDATE(), 2),
('Seasonal Favorites', GETDATE(), 3),
('Vegan Options', GETDATE(), 4),
('Pizza Classics', GETDATE(), 5);

-- Insert into Users Table
INSERT INTO Users (Name, Email, PasswordHash, PhoneNumber, Address, City, Country, CreatedAt)
VALUES 
('John Doe', 'john@example.com', 'AQAAAAIAAYagAAAAEPfUBJ561e+oUyjMrCiEb+LDljs56G46BwncRbhL5MG0URMm91d24ESspK2B3c0CoA==', '123456789', '123 Main St', 'Lyon', 'France', GETDATE()),
('Jane Smith', 'jane@example.com', 'AQAAAAIAAYagAAAAEPfUBJ561e+oUyjMrCiEb+LDljs56G46BwncRbhL5MG0URMm91d24ESspK2B3c0CoA==', '987654321', '456 Elm St', 'Lyon', 'France', GETDATE()),
('Mark Miller', 'mark@example.com', 'AQAAAAIAAYagAAAAEPfUBJ561e+oUyjMrCiEb+LDljs56G46BwncRbhL5MG0URMm91d24ESspK2B3c0CoA==', '111222333', '789 Oak St', 'Lyon', 'France', GETDATE());

-- Insert into PromotionCards Table
INSERT INTO PromotionCards (CardCode, DiscountAmount, ExpiryDate, MinAmount, DiscountType)
VALUES 
('SAVE10', 10.00, '2024-12-31', 20, 'Amount'),
('DISCOUNT5', 5.00, '2024-10-31', 15, 'Amount');

-- Insert into Food Items Table
INSERT INTO FoodItems (Ratings, NumOfReviews, Name, Description, Price, IsAvailable, CreatedAt, Id_Categories, Id_Menus)
VALUES 
(4.8, 180, 'Margherita Pizza', 'Classic margherita pizza with mozzarella cheese and tomato sauce', 12.99, 1, GETDATE(), 4, 5),
(4.7, 170, 'Pepperoni Pizza', 'Spicy pepperoni pizza with mozzarella cheese and tomato sauce', 14.99, 1, GETDATE(), 4, 5),
(4.6, 150, 'Cheeseburger', 'Juicy cheeseburger with lettuce, tomato, and pickles', 9.99, 1, GETDATE(), 3, 3),
(4.5, 140, 'Bacon Burger', 'Crispy bacon burger with melted cheddar and BBQ sauce', 11.99, 1, GETDATE(), 3, 3),
(4.9, 200, 'California Roll', 'Fresh sushi roll with crab, avocado, and cucumber', 8.99, 1, GETDATE(), 5, 1),
(4.8, 180, 'Spicy Tuna Roll', 'Spicy tuna roll with cucumber and sesame seeds', 10.99, 1, GETDATE(), 5, 1),
-- Chinese
(4.7, 120, 'Kung Pao Chicken', 'Stir-fried chicken with peanuts, chili peppers, and vegetables.', 12.99, 1, GETDATE(), 1, 5),
(4.5, 90, 'Sweet and Sour Pork', 'Pork in a tangy sweet and sour sauce.', 10.99, 1, GETDATE(), 1, 5),
(4.8, 150, 'Spring Rolls', 'Crispy rolls stuffed with vegetables.', 5.99, 1, GETDATE(), 1, 5),
(4.6, 110, 'Beef Chow Mein', 'Noodles stir-fried with beef and vegetables.', 11.99, 1, GETDATE(), 1, 5),
(4.4, 75, 'Mapo Tofu', 'Spicy tofu with minced pork and chili sauce.', 9.99, 1, GETDATE(), 1, 5),
-- Dessert
(4.9, 200, 'Chocolate Lava Cake', 'Rich chocolate cake with molten center.', 7.99, 1, GETDATE(), 2, 3),
(4.8, 180, 'Cheesecake', 'Creamy cheesecake with a graham cracker crust.', 6.99, 1, GETDATE(), 2, 3),
(4.7, 160, 'Tiramisu', 'Classic Italian dessert with coffee and mascarpone.', 8.99, 1, GETDATE(), 2, 3),
(4.6, 140, 'Macarons', 'Colorful almond meringue cookies.', 9.99, 1, GETDATE(), 2, 3),
(4.5, 100, 'Fruit Tart', 'Tart with creamy custard and fresh fruits.', 7.99, 1, GETDATE(), 2, 3),
-- Fast Food
(4.3, 90, 'Double Cheeseburger', 'Beef burger with double cheese.', 8.99, 1, GETDATE(), 3, 1),
(4.4, 85, 'Chicken Nuggets', 'Crispy chicken nuggets.', 6.99, 1, GETDATE(), 3, 1),
(4.5, 100, 'Fries', 'Crispy golden fries.', 2.99, 1, GETDATE(), 3, 1),
(4.2, 70, 'Onion Rings', 'Crispy battered onion rings.', 3.99, 1, GETDATE(), 3, 1),
(4.1, 50, 'Hot Dog', 'Grilled sausage in a bun.', 5.99, 1, GETDATE(), 3, 1),
-- Italian
(4.8, 160, 'Fettuccine Alfredo', 'Pasta in a creamy Alfredo sauce.', 12.99, 1, GETDATE(), 4, 2),
(4.7, 140, 'Lasagna', 'Layers of pasta, meat, and cheese.', 13.99, 1, GETDATE(), 4, 2),
(4.6, 130, 'Margherita Pizza', 'Classic pizza with tomato and mozzarella.', 11.99, 1, GETDATE(), 4, 2),
(4.5, 120, 'Penne Arrabiata', 'Pasta with spicy tomato sauce.', 10.99, 1, GETDATE(), 4, 2),
(4.4, 100, 'Bruschetta', 'Grilled bread with tomato topping.', 6.99, 1, GETDATE(), 4, 2),
-- Japanese
(4.9, 180, 'California Roll', 'Sushi roll with crab and avocado.', 8.99, 1, GETDATE(), 5, 2),
(4.8, 170, 'Spicy Tuna Roll', 'Sushi roll with spicy tuna.', 10.99, 1, GETDATE(), 5, 2),
(4.7, 160, 'Ramen', 'Noodle soup with pork and egg.', 12.99, 1, GETDATE(), 5, 2),
(4.6, 150, 'Tempura', 'Crispy battered seafood or vegetables.', 9.99, 1, GETDATE(), 5, 2),
(4.5, 140, 'Miso Soup', 'Soup with tofu and seaweed.', 3.99, 1, GETDATE(), 5, 2),
-- Snack
(4.3, 100, 'Chips', 'Crispy potato chips.', 1.99, 1, GETDATE(), 6, 1),
(4.4, 95, 'Nachos', 'Tortilla chips with cheese and toppings.', 6.99, 1, GETDATE(), 6, 1),
-- Spicy
(4.7, 120, 'Spicy Chicken Wings', 'Crispy wings with spicy sauce.', 9.99, 1, GETDATE(), 7, 2),
-- Vegan
(4.9, 150, 'Vegan Burger', 'Plant-based burger.', 10.99, 1, GETDATE(), 8, 4),
(4.8, 130, 'Vegan Salad', 'Salad with fresh greens and nuts.', 8.99, 1, GETDATE(), 8, 4);

-- Insert into UserCards Table
INSERT INTO UserCards (IsUsed, Id_Users, Id_PromotionCards)
VALUES 
(1, 1, 1),  -- User 1 uses Promotion Card 1
(0, 2, 2), -- User 2 uses Promotion Card 2
(1, 3, 1),  -- User 3 uses Promotion Card 1
(0, 1, 2);  -- User 1 uses Promotion Card 2

-- Insert into Orders Table
INSERT INTO Orders (Id_Users, TotalAmount, OrderStatus, OrderDate, CreatedAt, Id_DeliveryGuys, Id_PromotionCards)
VALUES 
(1, 25.98, 'Pending', GETDATE(), GETDATE(), 1, NULL),
(2, 21.98, 'Confirmed', GETDATE(), GETDATE(), 2, 1),
(3, 19.98, 'Delivered', GETDATE(), GETDATE(), NULL, 2);

-- Insert into Order Items Table
INSERT INTO OrderItems (Quantity, Price, Id_FoodItems, Id_Orders)
VALUES 
(1, 12.99, 27, 2),
(1, 12.99, 28, 2),
(2, 9.99, 29, 3),
(1, 11.99, 30, 3),
(1, 8.99, 31, 4),
(1, 10.99, 31, 4);

-- Insert into Delivery Guys Table
INSERT INTO DeliveryGuys (Name, PhoneNumber, Email, City, Country, Availability, VehiculeType, CreatedAt)
VALUES 
('Tom Driver', '555222111', 'tomdriver@example.com', 'Lyon', 'France', 1, 'Bike', GETDATE()),
('Sarah Courier', '555333444', 'sarahcourier@example.com', 'Lyon', 'France', 1, 'Car', GETDATE());
