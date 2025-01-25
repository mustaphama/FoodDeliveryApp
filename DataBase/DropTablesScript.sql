-- Drop all foreign key constraints first
-- If constraints need to be removed explicitly:
-- Run this before dropping tables

DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
               ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.foreign_keys;

EXEC sp_executesql @sql;

-- Drop all tables in the correct order
DROP TABLE IF EXISTS OrderItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS UserCards;
DROP TABLE IF EXISTS PromotionCards;
DROP TABLE IF EXISTS FoodItems;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Menus;
DROP TABLE IF EXISTS Restaurants;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS DeliveryGuys;

-- All tables have been dropped successfully
