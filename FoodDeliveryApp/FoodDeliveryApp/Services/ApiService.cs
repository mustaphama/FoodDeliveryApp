using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Or use System.Text.Json
using System.Net.Http.Headers;
using System.Diagnostics;
using FoodDeliveryApp.Models;
using System.Text.Json;

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
    public async Task<bool> RegisterUser(string name, string email, string password, string phoneNumber, string address, string profilePictureUrl)
    {
        var registerData = new
        {
            Name = name,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber,
            Address = address,
            ProfilePictureUrl = profilePictureUrl
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

    // Method to Log In a User and retrieve UserId
    public async Task<(bool IsSuccess, string ErrorMessage, int UserId)> LoginUser(string email, string password)
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

                // Deserialize response to extract UserId
                var jsonResponse = JsonConvert.DeserializeObject<Models>(responseData);

                if (jsonResponse != null && jsonResponse.UserId > 0)
                {
                    return (true, "Login successful!", jsonResponse.UserId);  // Successful login with UserId
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

    public async Task<List<HotProduct>> GetMostOrderedProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Home/GetMostOrderedProducts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HotProduct>>(json);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch products: {response.StatusCode}");
                return new List<HotProduct>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Request Exception: {ex.Message}");
            return new List<HotProduct>();
        }
    }
    public async Task<Product> GetProductById(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/FoodItems/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(jsonResponse);
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

}
