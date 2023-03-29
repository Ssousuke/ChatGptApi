using ChatGptApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace ChatGptApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGptController : ControllerBase
    {

        [AcceptVerbs("GET", "POST")]
        [Route("Chat/{message}")]
        public async Task<string> PostGtp(string message)
        {
            var response = await GetChatGptResponse(message);
            return response;
        }

        public async Task<string> GetChatGptResponse(string message)
        {
            // 
            string apiKey = "SECRET-KEY";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            string url = "https://api.openai.com/v1/completions";
            string prompt = message;
            string payload = $@"{{                                
                             ""model"": ""text-davinci-003"",
                              ""prompt"": ""{prompt}"",
                              ""max_tokens"": 500,
                              ""temperature"": 1.0,
                              ""top_p"": 1,
                              ""n"": 1,
                              ""stream"": false,
                              ""logprobs"": null
                            }}";
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            string result = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<ChatGptResponse>(result);

            var choice = jsonResult.choices.FirstOrDefault();
            if (choice != null && choice.text != null)
            {
                return choice.text;
            }
            else
            {
                return "Erro: resposta inválida do modelo.";
            }
        }
    }
}