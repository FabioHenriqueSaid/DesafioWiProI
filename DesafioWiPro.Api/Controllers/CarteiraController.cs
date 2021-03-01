using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DesafioWiPro.Data.Context;
using DesafioWiPro.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Threading;

namespace DesafioWiPro.Api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("carteira")]
    public class CarteiraController : ControllerBase
    {
        private ProducerConfig _config;
        public CarteiraController(ProducerConfig config)
        {
            this._config = config;
        }
        [HttpPost("send")]
        public async Task<ActionResult> AddItemFila(string topic, [FromBody] Carteira model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string serializedEmployee = JsonConvert.SerializeObject(model);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedEmployee });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetItemFila()
        {
            var configConsumer = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(configConsumer).Build())
            {
                consumer.Subscribe("test-topic");
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };
                while (true)
                {

                    var cr = consumer.Consume(cts.Token);
                    if (cr == null)
                    {
                        return BadRequest(new { message = "Nenhum objeto encontrado", });
                    }
                    else
                    {
                        string mensagem = cr.Message.Value.Replace("\"", "");
                        return Ok(new { mensagem });
                     
                    }
                }
            }

        }



        //[HttpGet]
        //public async Task<ActionResult<Carteira>> Get([FromServices] DataContext context)
        //{
        //    var moedas = context.Carteiras.AsNoTracking()
        //           .OrderByDescending(c => c.Id)
        //           .FirstOrDefault();
        //    if (moedas == null)
        //    {
        //        return BadRequest(new { message = "Nenhum objeto encontrado", });
        //    }

        //    return Ok(moedas);
        //}


        //[HttpPost]
        //[Microsoft.AspNetCore.Mvc.Route("")]
        //[AllowAnonymous]
        //public async Task<ActionResult<Carteira>> AddItemFila([FromServices] DataContext context, [FromBody] Carteira model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        context.Carteiras.Add(model);
        //        await context.SaveChangesAsync();
        //        return Ok(new { message = "Moeda criada" });
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new { message = "Não foi possível criar a moeda", e });

        //    }
        //}

    }
}
