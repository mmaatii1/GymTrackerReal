﻿using GymTracker.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using GymTracker.Shared;

namespace GymTracker.Services
{
    public class RestService<TEntity> : IRestService<TEntity> where TEntity : class
    {
        HttpClient _client;
        IHttpsClientHandlerService _httpsClientHandlerService;
        private readonly IKeyVaultService _keyVaultService;
        private readonly string ApiUrl;
        public List<TEntity> Items { get; private set; }

        public RestService(IHttpsClientHandlerService service,IKeyVaultService keyVault)
        {
#if DEBUG
            _httpsClientHandlerService = service;
            HttpMessageHandler handler = _httpsClientHandlerService.GetPlatformMessageHandler();
            if (handler != null)
                _client = new HttpClient(handler);
            else
                _client = new HttpClient();
#else
            _client = new HttpClient();
#endif
          _keyVaultService = keyVault;
          ApiUrl = _keyVaultService.GetSecret("ApiUrl").Result;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            Items = new List<TEntity>();

            Uri uri = new Uri(string.Format(ApiUrl + typeof(TEntity).Name, string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<TEntity>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task<TEntity> SaveAsync(TEntity item, bool isNewItem = false, bool isPicture = false)
        {
            try
            {
                string apiEndpoint = typeof(TEntity).GetCustomAttribute<ApiEndpointAttribute>()?.EndpointName ?? typeof(TEntity).Name;
                Uri uri = new Uri(string.Format(ApiUrl + apiEndpoint, string.Empty));

                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if(isPicture)
                {
                    var result = await _client.PostAsync(uri, content);
                    result.EnsureSuccessStatusCode();
                }
                if (isNewItem)
                {
                    response = await _client.PostAsync(uri, content);
                    var read = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TEntity>(read);
                }
                else
                {
                    response = await _client.PutAsync(uri, content);
                    var read = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TEntity>(read);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                string apiEndpoint =  typeof(TEntity).Name;
                Uri uri = new Uri(string.Format(ApiUrl + apiEndpoint +"/" + id, string.Empty));

                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"successfully deleted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            Uri uri = new Uri(string.Format(ApiUrl + typeof(TEntity).Name + "/" + id, string.Empty));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TEntity>(content);
                }
            }   
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }
    }
}
