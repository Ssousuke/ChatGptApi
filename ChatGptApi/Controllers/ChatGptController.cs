using ChatGptApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ChatGptApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGptController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ChatGptController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AcceptVerbs("GET", "POST")]
        [Route("Chat/{message?}")]
        public async Task<ActionResult<string>> PostGtp(string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "Nenhum parametro passado.";
            var response = await GetChatGptResponse(message);
            return response;
        }

        public async Task<string> GetChatGptResponse(string? message)
        {
            string? apiKey = _configuration.GetSection("ApiKey").Value;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            string? url = _configuration.GetSection("OpenAIUrl").Value;
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