using GymTracker.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace GymTracker.Services
{
    public class RestService<TEntity> : IRestService<TEntity> where TEntity : class
    {
        HttpClient _client;
        IHttpsClientHandlerService _httpsClientHandlerService;

        public List<TEntity> Items { get; private set; }

        public RestService(IHttpsClientHandlerService service)
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
          
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            Items = new List<TEntity>();

            Uri uri = new Uri(string.Format(Constants.RestUrl + typeof(TEntity).Name, string.Empty));
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

        public async Task<TEntity> SaveAsync(TEntity item, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl + typeof(TEntity).Name, string.Empty));

            if (typeof(TEntity).Equals(typeof(SpecificExerciseUpdateCreateDto)))
            {
                uri = new Uri(string.Format(Constants.RestUrl + "SpecificExercise", string.Empty));
            }

            if (typeof(TEntity).Equals(typeof(CustomWorkoutCreateUpdateDto)))
            {
                uri = new Uri(string.Format(Constants.RestUrl + "CustomWorkout", string.Empty));
            }

            try
            {
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
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

        public async Task DeleteAsync(string id)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, id));

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tTodoItem successfully deleted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
