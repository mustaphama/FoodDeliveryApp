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
INSERT INTO FoodItems (MenuId, CategoryId, Name, Description, Price, ImageUrl)
VALUES 
(1, 4, 'Margherita Pizza', 'Classic margherita pizza with mozzarella cheese and tomato sauce', 12.99, 'https://example.com/images/fooditems/margherita.jpg'),
(1, 4, 'Pepperoni Pizza', 'Spicy pepperoni pizza with mozzarella cheese and tomato sauce', 14.99, 'https://example.com/images/fooditems/pepperoni.jpg'),
(2, 1, 'Cheeseburger', 'Juicy cheeseburger with lettuce, tomato, and pickles', 9.99, 'https://example.com/images/fooditems/cheeseburger.jpg'),
(2, 1, 'Bacon Burger', 'Crispy bacon burger with melted cheddar and BBQ sauce', 11.99, 'https://example.com/images/fooditems/baconburger.jpg'),
(3, 5, 'California Roll', 'Fresh sushi roll with crab, avocado, and cucumber', 8.99, 'https://example.com/images/fooditems/californiaroll.jpg'),
(3, 5, 'Spicy Tuna Roll', 'Spicy tuna roll with cucumber and sesame seeds', 10.99, 'https://example.com/images/fooditems/spicytuna.jpg');

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
