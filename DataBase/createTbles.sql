-- Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,  -- Unique constraint
    PasswordHash NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    ProfilePictureUrl VARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Restaurants Table
CREATE TABLE Restaurants (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Location NVARCHAR(255),
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    LogoUrl VARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Categories Table
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) UNIQUE  -- Unique constraint
);

-- Menus Table
CREATE TABLE Menus (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(Id),
    MenuName NVARCHAR(100),
    MenuImageUrl VARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Food Items Table
CREATE TABLE FoodItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MenuId INT FOREIGN KEY REFERENCES Menus(Id),
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
    Name NVARCHAR(100),
    Description NVARCHAR(255),
    Price DECIMAL(10, 2),
    ImageUrl VARCHAR(255),
    IsAvailable BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);


-- Delivery Guys Table
CREATE TABLE DeliveryGuys (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    Availability BIT DEFAULT 1,
    VehicleType NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Promotion Cards Table
CREATE TABLE PromotionCards (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CardCode NVARCHAR(50) UNIQUE,
    DiscountAmount DECIMAL(5, 2),
    IsUsed BIT DEFAULT 0,
    ExpiryDate DATETIME
);

-- Orders Table
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(Id),
    TotalAmount DECIMAL(10, 2),
    OrderStatus NVARCHAR(50) NOT NULL CHECK (OrderStatus IN ('Pending', 'Confirmed', 'Preparing', 'Ready for Pickup', 'Out for Delivery', 'Delivered', 'Completed', 'Cancelled')),
    OrderDate DATETIME DEFAULT GETDATE(),
    DeliveryGuyId INT FOREIGN KEY REFERENCES DeliveryGuys(Id),
    PromotionCardId INT FOREIGN KEY REFERENCES PromotionCards(Id),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Order Items Table
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    FoodItemId INT FOREIGN KEY REFERENCES FoodItems(Id),
    Quantity INT,
    Price DECIMAL(10, 2)
);

-- Payments Table
CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    PaymentDate DATETIME DEFAULT GETDATE()
);
