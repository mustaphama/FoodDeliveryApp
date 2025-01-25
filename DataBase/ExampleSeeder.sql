-- Insert into Users Table
INSERT INTO Users (Name, Email, PasswordHash, PhoneNumber, Address, ProfilePictureUrl)
VALUES 
('John Doe', 'john@example.com', 'passwordhash123', '123456789', '123 Main St', 'https://example.com/images/users/john.jpg'),
('Jane Smith', 'jane@example.com', 'passwordhash456', '987654321', '456 Elm St', 'https://example.com/images/users/jane.jpg'),
('Mark Miller', 'mark@example.com', 'passwordhash789', '111222333', '789 Oak St', 'https://example.com/images/users/mark.jpg');

-- Insert into Restaurants Table
INSERT INTO Restaurants (Name, Location, Email, PhoneNumber, LogoUrl)
VALUES 
('Pizza Palace', '12 Pizza Ave', 'contact@pizzapalace.com', '555123456', 'https://example.com/images/restaurants/pizzapalace.jpg'),
('Burger House', '34 Burger Blvd', 'contact@burgerhouse.com', '555654321', 'https://example.com/images/restaurants/burgerhouse.jpg'),
('Sushi World', '56 Sushi St', 'contact@sushiworld.com', '555987654', 'https://example.com/images/restaurants/sushiworld.jpg');

-- Insert into Categories Table
INSERT INTO Categories (CategoryName)
VALUES 
('Snack'), 
('Dessert'), 
('Chinese'), 
('Italian'), 
('Japanese');

-- Insert into Menus Table
INSERT INTO Menus (RestaurantId, MenuName, MenuImageUrl)
VALUES 
(1, 'Pizza Specials', 'https://example.com/images/menus/pizzaspecials.jpg'),
(2, 'Burger Combos', 'https://example.com/images/menus/burgercombos.jpg'),
(3, 'Sushi Rolls', 'https://example.com/images/menus/sushirolls.jpg');

-- Insert into Food Items Table
INSERT INTO FoodItems (Ratings, NumOfReviews, Name, Description, Price, IsAvailable, CreatedAt, Id_Categories, Id_Menus)
VALUES 
(4.8, 180, 'Margherita Pizza', 'Classic margherita pizza with mozzarella cheese and tomato sauce', 12.99, TRUE, NOW(), 4, 9),
(4.7, 170, 'Pepperoni Pizza', 'Spicy pepperoni pizza with mozzarella cheese and tomato sauce', 14.99, TRUE, NOW(), 4, 9),
(4.6, 150, 'Cheeseburger', 'Juicy cheeseburger with lettuce, tomato, and pickles', 9.99, TRUE, NOW(), 3, 10),
(4.5, 140, 'Bacon Burger', 'Crispy bacon burger with melted cheddar and BBQ sauce', 11.99, TRUE, NOW(), 3, 10),
(4.9, 200, 'California Roll', 'Fresh sushi roll with crab, avocado, and cucumber', 8.99, TRUE, NOW(), 5, 11),
(4.8, 180, 'Spicy Tuna Roll', 'Spicy tuna roll with cucumber and sesame seeds', 10.99, TRUE, NOW(), 5, 11),
-- Chinese
(4.7, 120, 'Kung Pao Chicken', 'Stir-fried chicken with peanuts, chili peppers, and vegetables.', 12.99, TRUE, NOW(), 1, 9),
(4.5, 90, 'Sweet and Sour Pork', 'Pork in a tangy sweet and sour sauce.', 10.99, TRUE, NOW(), 1, 9),
(4.8, 150, 'Spring Rolls', 'Crispy rolls stuffed with vegetables.', 5.99, TRUE, NOW(), 1, 9),
(4.6, 110, 'Beef Chow Mein', 'Noodles stir-fried with beef and vegetables.', 11.99, TRUE, NOW(), 1, 9),
(4.4, 75, 'Mapo Tofu', 'Spicy tofu with minced pork and chili sauce.', 9.99, TRUE, NOW(), 1, 9),

-- Dessert
(4.9, 200, 'Chocolate Lava Cake', 'Rich chocolate cake with molten center.', 7.99, TRUE, NOW(), 2, 10),
(4.8, 180, 'Cheesecake', 'Creamy cheesecake with a graham cracker crust.', 6.99, TRUE, NOW(), 2, 10),
(4.7, 160, 'Tiramisu', 'Classic Italian dessert with coffee and mascarpone.', 8.99, TRUE, NOW(), 2, 10),
(4.6, 140, 'Macarons', 'Colorful almond meringue cookies.', 9.99, TRUE, NOW(), 2, 10),
(4.5, 100, 'Fruit Tart', 'Tart with creamy custard and fresh fruits.', 7.99, TRUE, NOW(), 2, 10),

-- Fast Food
(4.3, 90, 'Double Cheeseburger', 'Beef burger with double cheese.', 8.99, TRUE, NOW(), 3, 11),
(4.4, 85, 'Chicken Nuggets', 'Crispy chicken nuggets.', 6.99, TRUE, NOW(), 3, 11),
(4.5, 100, 'Fries', 'Crispy golden fries.', 2.99, TRUE, NOW(), 3, 11),
(4.2, 70, 'Onion Rings', 'Crispy battered onion rings.', 3.99, TRUE, NOW(), 3, 11),
(4.1, 50, 'Hot Dog', 'Grilled sausage in a bun.', 5.99, TRUE, NOW(), 3, 11),

-- Italian
(4.8, 160, 'Fettuccine Alfredo', 'Pasta in a creamy Alfredo sauce.', 12.99, TRUE, NOW(), 4, 12),
(4.7, 140, 'Lasagna', 'Layers of pasta, meat, and cheese.', 13.99, TRUE, NOW(), 4, 12),
(4.6, 130, 'Margherita Pizza', 'Classic pizza with tomato and mozzarella.', 11.99, TRUE, NOW(), 4, 12),
(4.5, 120, 'Penne Arrabiata', 'Pasta with spicy tomato sauce.', 10.99, TRUE, NOW(), 4, 12),
(4.4, 100, 'Bruschetta', 'Grilled bread with tomato topping.', 6.99, TRUE, NOW(), 4, 12),

-- Japanese
(4.9, 180, 'California Roll', 'Sushi roll with crab and avocado.', 8.99, TRUE, NOW(), 5, 13),
(4.8, 170, 'Spicy Tuna Roll', 'Sushi roll with spicy tuna.', 10.99, TRUE, NOW(), 5, 13),
(4.7, 160, 'Ramen', 'Noodle soup with pork and egg.', 12.99, TRUE, NOW(), 5, 13),
(4.6, 150, 'Tempura', 'Crispy battered seafood or vegetables.', 9.99, TRUE, NOW(), 5, 13),
(4.5, 140, 'Miso Soup', 'Soup with tofu and seaweed.', 3.99, TRUE, NOW(), 5, 13),

-- Snack
(4.3, 100, 'Chips', 'Crispy potato chips.', 1.99, TRUE, NOW(), 6, 11),
(4.4, 95, 'Nachos', 'Tortilla chips with cheese and toppings.', 6.99, TRUE, NOW(), 6, 11),

-- Spicy
(4.7, 120, 'Spicy Chicken Wings', 'Crispy wings with spicy sauce.', 9.99, TRUE, NOW(), 7, 14),

-- Vegan
(4.9, 150, 'Vegan Burger', 'Plant-based burger.', 10.99, TRUE, NOW(), 8, 15),
(4.8, 130, 'Vegan Salad', 'Salad with fresh greens and nuts.', 8.99, TRUE, NOW(), 8, 15);


-- Insert into Delivery Guys Table
INSERT INTO DeliveryGuys (Name, PhoneNumber, Email, VehicleType)
VALUES 
('Tom Driver', '555222111', 'tomdriver@example.com', 'Bike'),
('Sarah Courier', '555333444', 'sarahcourier@example.com', 'Car');

-- Insert into Promotion Cards Table
INSERT INTO PromotionCards (CardCode, DiscountAmount, ExpiryDate)
VALUES 
('SAVE10', 10.00,2024-12-31),
('DISCOUNT5', 5.00,2024-10-31);

-- Insert into Orders Table
INSERT INTO Orders (UserId, RestaurantId, TotalAmount, OrderStatus, DeliveryGuyId, PromotionCardId)
VALUES 
(1, 1, 25.98, 'Pending', 1, NULL),
(2, 2, 21.98, 'Sent', 2, 5),
(3, 3, 19.98, 'Delivered', NULL, 6);

-- Insert into Order Items Table
INSERT INTO OrderItems (OrderId, FoodItemId, Quantity, Price)
VALUES 
(20, 1, 1, 12.99),
(20, 2, 1, 12.99),
(21, 3, 2, 9.99),
(21, 4, 1, 11.99),
(22, 5, 1, 8.99),
(22, 6, 1, 10.99);
