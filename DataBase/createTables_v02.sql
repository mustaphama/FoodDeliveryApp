-- Create Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Restaurants table
CREATE TABLE Restaurants (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Menus table
CREATE TABLE Menus (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Id_Restaurants INT NOT NULL FOREIGN KEY REFERENCES Restaurants(Id)
);

-- Create Categories table
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);

-- Create FoodItems table
CREATE TABLE FoodItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ratings FLOAT NOT NULL,
    NumOfReviews INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Id_Categories INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
    Id_Menus INT NOT NULL FOREIGN KEY REFERENCES Menus(Id)
);

-- Create PromotionCards table
CREATE TABLE PromotionCards (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CardCode NVARCHAR(50) NOT NULL UNIQUE,
    DiscountAmount DECIMAL(10,2) NOT NULL,
    ExpiryDate DATE NOT NULL,
    MinAmount DECIMAL(10,2) NOT NULL,
    DiscountType NVARCHAR(50) NOT NULL
);

-- Create UserCards table
CREATE TABLE UserCards (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsUsed BIT NOT NULL,
    Id_Users INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Id_PromotionCards INT FOREIGN KEY REFERENCES PromotionCards(Id)
);

-- Create DeliveryGuys table
CREATE TABLE DeliveryGuys (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    City NVARCHAR(100),
    Country NVARCHAR(100),
    Availability BIT NOT NULL DEFAULT 1,
    VehiculeType NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Orders table
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Id_Users INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus NVARCHAR(50) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Id_DeliveryGuys INT FOREIGN KEY REFERENCES DeliveryGuys(Id),
    Id_PromotionCards INT FOREIGN KEY REFERENCES PromotionCards(Id)
);

-- Create OrderItems table
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Id_FoodItems INT NOT NULL FOREIGN KEY REFERENCES FoodItems(Id),
    Id_Orders INT NOT NULL FOREIGN KEY REFERENCES Orders(Id)
);

ALTER TABLE Orders
ADD CONSTRAINT chk_OrderStatus
CHECK (OrderStatus IN ('Pending', 'Confirmed', 'Preparing', 'Ready for Pickup', 'Out for Delivery', 'Delivered', 'Completed', 'Cancelled'));

ALTER TABLE PromotionCards
ADD CONSTRAINT chk_DiscountType
CHECK (DiscountType IN ('Amount', 'Percentage'));

ALTER TABLE UserCards
ADD CONSTRAINT UC_UserCards_UserId_PromotionId UNIQUE (Id_Users, Id_PromotionCards);

ALTER TABLE Users
ADD Latitude DECIMAL(9,9) NULL,
    Longitude DECIMAL(9,9) NULL;

