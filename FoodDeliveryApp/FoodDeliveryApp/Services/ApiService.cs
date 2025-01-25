﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Or use System.Text.Json
using System.Net.Http.Headers;
using System.Diagnostics;
using FoodDeliveryApp.Models;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        // Create a custom HttpClientHandler to bypass SSL validation (for testing only)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://172.16.1.114:7167/") // Replace with your API base URL
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // Method to Register a New User
    public async Task<bool> RegisterUser(string name, string email, string password, string phoneNumber, string address, string city, string country)
    {
        var registerData = new
        {
            Name = name,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber,
            Address = address,
            City = city,
            Country = country
        };

        var json = JsonConvert.SerializeObject(registerData);  // Serialize data to JSON
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("api/Auth/register", content); // Call the API
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Registration Request Exception: {ex.Message}");
            return false;  // Registration failed
        }
    }

    // Method to Log In a User and retrieve Id_Users
    public async Task<(bool IsSuccess, string ErrorMessage, int Id_Users)> LoginUser(string email, string password)
    {
        var loginData = new
        {
            Email = email,
            Password = password
        };

        var json = JsonConvert.SerializeObject(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("api/Auth/login", content);  // Call the API

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Deserialize response to extract Id_Users
                var jsonResponse = JsonConvert.DeserializeObject<Models>(responseData);

                if (jsonResponse != null && jsonResponse.Id_Users > 0)
                {
                    return (true, "Login successful!", jsonResponse.Id_Users);  // Successful login with Id_Users
                }
                else
                {
                    return (false, "Login successful but failed to retrieve UserId.", 0);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Login failed with status code: {response.StatusCode}, Response: {errorContent}");
                return (false, "Login failed. Please check your credentials.", 0);
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Login Request Exception: {ex.Message}");
            return (false, "Unable to connect to the server.", 0);
        }
    }
    public async Task<List<MostOrderedCategory>> GetMostOrderedCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Home/GetMostOrderedCategories");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MostOrderedCategory>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch categories: {response.StatusCode}");
                return new List<MostOrderedCategory>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<MostOrderedCategory>();
        }
    }

    public async Task<List<FoodItemDto>> GetMostOrderedProductsAsync(int userId)
    {
        try
        {
            // Add userId as a query parameter to the API request
            var response = await _httpClient.GetAsync($"api/Home/GetMostOrderedProducts?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemDto>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<FoodItemDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<FoodItemDto>();
        }
    }
    public async Task<List<FoodItemDto>> GetHighestRatedProductsAsync(int userId)
    {
        try
        {
            // Add userId as a query parameter to the API request
            var response = await _httpClient.GetAsync($"api/Home/GetHighestRatedProducts?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemDto>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<FoodItemDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<FoodItemDto>();
        }
    }
    public async Task<List<FoodItemDto>> GetNewestFoodItemsAsync(int userId)
    {
        try
        {
            // Add userId as a query parameter to the API request
            var response = await _httpClient.GetAsync($"api/Home/GetNewestFoodItems?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemDto>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<FoodItemDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<FoodItemDto>();
        }
    }
    public async Task<List<FoodItemDto>> GetCheapestFoodItemsAsync(int userId)
    {
        try
        {
            // Add userId as a query parameter to the API request
            var response = await _httpClient.GetAsync($"api/Home/GetCheapestFoodItems?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemDto>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<FoodItemDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<FoodItemDto>();
        }
    }

    public async Task<FoodItemDto> GetProductById(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/FoodItems/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FoodItemDto>(jsonResponse);
            }
            else
            {
                throw new Exception($"Failed to fetch product. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching product by ID: {ex.Message}");
            throw;
        }
    }
    public async Task<List<FoodItemWithRestaurantDto>> GetFoodItemsByIdsAsync(List<int> ids)
    {
        try
        {
            var jsonContent = JsonConvert.SerializeObject(ids);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/CartPage/GetByIds", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemWithRestaurantDto>>(jsonResponse);
            }
            else
            {
                throw new Exception($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching products by IDs: {ex.Message}");
            throw;
        }
    }
    // Fetch user profile details from API by Id_Users
    public async Task<User> GetUserProfileAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(json);
                return user;
            }
            else
            {
                throw new Exception("Failed to fetch user profile data.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user profile: {ex.Message}");
        }
    }
    public async Task<string> GetUserAddressAsync(string userId)
{
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(json);
                return user.Address;
            }
            else
            {
                throw new Exception("Failed to fetch user profile data.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user profile: {ex.Message}");
        }
    }
    public async Task<string> GetUserPhoneAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(json);
                return user.PhoneNumber;
            }
            else
            {
                throw new Exception("Failed to fetch user profile data.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user profile: {ex.Message}");
        }
    }
    public async Task<bool> UpdateUserAddressAsync(string userId, string newAddress)
    {
        try
        {
            // Create a payload with the new address
            var payload = new { Address = newAddress };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Create a StringContent object with the JSON payload
            var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            // Make the PUT request to update the user's address
            var response = await _httpClient.PutAsync($"api/Users/{userId}/address", content);

            if (response.IsSuccessStatusCode)
            {
                return true; // Address updated successfully
            }
            else
            {
                throw new Exception("Failed to update user address.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user address: {ex.Message}");
        }
    }
    public async Task<bool> UpdateUserPhoneAsync(string userId, string newPhone)
    {
        try
        {
            // Create a payload with the new address
            var payload = new { Phone = newPhone };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Create a StringContent object with the JSON payload
            var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            // Make the PUT request to update the user's address
            var response = await _httpClient.PutAsync($"api/Users/{userId}/phoneNumber", content);

            if (response.IsSuccessStatusCode)
            {
                return true; // Address updated successfully
            }
            else
            {
                throw new Exception("Failed to update user phone number.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user phone number: {ex.Message}");
        }
    }
    public async Task<(bool IsSuccess, string ErrorMessage)> ModifyPasswordAsync(int userId, string oldPassword, string newPassword)
    {
        try
        {
            // Prepare the payload with the old and new passwords
            var payload = new
            {
                UserId = userId,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Create a StringContent object with the JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Make the PUT request to modify the password
            var response = await _httpClient.PutAsync("api/Auth/modifyPassword", content);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Password updated successfully.");
            }
            else
            {
                // Read the error message from the response content
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to update password. Server response: {errorContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return (false, "Unable to connect to the server.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected Exception: {ex.Message}");
            return (false, "An unexpected error occurred.");
        }
    }
    public async Task<(bool IsSuccess, string ErrorMessage)> ModifyEmailAsync(int userId, string newEmail, string password)
    {
        try
        {
            var requestData = new
            {
                UserId = userId,
                NewEmail = newEmail,
                Password = password
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Auth/modifyEmail", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<String>(responseData);
                return (true, apiResponse);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error: {response.StatusCode}, {errorContent}");
                return (false, "Failed to update email.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating email: {ex.Message}");
            return (false, "An error occurred while updating the email.");
        }
    }
    public async Task<string> GetUserEmailAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/getEmail/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var email = await response.Content.ReadAsStringAsync();
                return email;
            }
            else
            {
                throw new Exception("Failed to fetch current email.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user email: {ex.Message}");
        }
    }
    public async Task<(bool IsSuccess, string ErrorMessage)> ModifyNameAsync(int userId, string newName, string password)
    {
        try
        {
            var requestData = new
            {
                UserId = userId,
                NewName = newName,
                Password = password
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Auth/modifyName", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<String>(responseData);
                return (true, apiResponse);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error: {response.StatusCode}, {errorContent}");
                return (false, "Failed to update name.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating name: {ex.Message}");
            return (false, "An error occurred while updating the name.");
        }
    }
    public async Task<(bool IsSuccess, string ErrorMessage, int OrderId)> PlaceOrderAsync(CartRequest cartRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Orders/PlaceOrder", cartRequest);
            if (response.IsSuccessStatusCode)
            {
                
                var rawResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Raw Response: {rawResponse}");
                var responseData = await response.Content.ReadFromJsonAsync<OrderResponse>();
                return (true, responseData.Message, responseData.Id_Orders); // Success, no error message, valid Id_Orders
            }
            else
            {
                var errorMessage = $"Error placing order: {response.StatusCode}";
                return (false, errorMessage, 0); // Failure, error message, invalid Id_Orders
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exception while placing order: {ex.Message}";
            Debug.WriteLine(errorMessage);
            return (false, errorMessage, 0); // Failure, error message, invalid Id_Orders
        }
    }
    public async Task<(bool IsSuccess, string Status, string ErrorMessage)> GetOrderStatusAsync(int orderId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/orders/{orderId}/status");

            if (response.IsSuccessStatusCode)
            {
                var orderStatus = await response.Content.ReadFromJsonAsync<string>();
                return (true, orderStatus, string.Empty);
            }
            else
            {
                var errorMessage = $"Error fetching order status: {response.StatusCode}";
                return (false, string.Empty, errorMessage);
            }
        }
        catch (Exception ex)
        {
            return (false, string.Empty, ex.Message);
        }
    }
    public async Task<PaginatedResponse<FoodItemDto>> GetFoodItemsByCategoryAsync(int categoryId, int pageNumber = 1, int pageSize = 8)
    {
        // Construct the URL with pagination parameters
        var url = $"api/Categories/{categoryId}/food-items?pageNumber={pageNumber}&pageSize={pageSize}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize the response into the PaginatedResponse<FoodItemDto> type
            return System.Text.Json.JsonSerializer.Deserialize<PaginatedResponse<FoodItemDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // Handle error response
        throw new Exception($"Failed to fetch food items. Status code: {response.StatusCode}");
    }
    public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/categories/{categoryId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryDto>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching category: {ex.Message}");
            return null;
        }
    }
    public async Task<List<FoodItemDto>> SearchFoodItemsAsync(string query)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/FoodItems/search?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<FoodItemDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            throw new Exception($"{response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while searching {ex.Message}");
            return null;
        }

    }
    public async Task<(bool IsSuccess, string ErrorMessage)> SubmitProductRatingAsync(RatingRequest ratingRequest)
    {
        try
        {
            // Make the HTTP POST request
            var response = await _httpClient.PostAsJsonAsync("api/FoodItems/submit-rating", ratingRequest);

            if (response.IsSuccessStatusCode)
            {
                // If the response indicates success, return true with no error message
                return (true, string.Empty);
            }
            else
            {
                // If the response is unsuccessful, read and return the error message
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent);
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP-specific exceptions
            return (false, $"HTTP Request Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            return (false, $"Unexpected Error: {ex.Message}");
        }
    }
    public async Task<RestaurantDto> GetRestaurantById(int restaurantId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Restaurant/{restaurantId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<RestaurantDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            throw new Exception($"{response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while fetching restaurant data {ex.Message}");
            return null;
        }

    }
    public async Task<List<Menu>> GetMenusByRestaurantId(int restaurantId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Menus/restaurants/{restaurantId}/menus");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<Menu>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            throw new Exception($"{response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while fetching Menu data {ex.Message}");
            return null;
        }

    }

    public async Task<List<FoodItemDto>> GetFoodItemsByMenuId(int menuId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/FoodItems/menus/{menuId}/fooditems");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<FoodItemDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            throw new Exception($"{response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while fetching FoodItem data {ex.Message}");
            return null;
        }

    }
    public async Task<List<FoodItemDto>> GetRecommendationsAsync(int foodItemId)
    {
        try
        {
            // Add foodItemId as a query parameter to the API request
            var response = await _httpClient.GetAsync($"api/FoodItems/{foodItemId}/recommendations");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItemDto>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<FoodItemDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<FoodItemDto>();
        }
    }
    public async Task<List<UserCards>> GetPromotionCardsByUserId(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/UserCards/User/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<UserCards>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            throw new Exception($"{response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while fetching Promotion Cards data {ex.Message}");
            return null;
        }

    }

}
