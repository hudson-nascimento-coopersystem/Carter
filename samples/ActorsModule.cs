namespace Botwin.Samples
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ActorsModule : BotwinModule
    {
        public ActorsModule(IActorProvider actorProvider)
        {
            this.Get("/actors", async (req, res, routeData) =>
            {
                var people = actorProvider.Get();
                await res.WriteAsync(JsonConvert.SerializeObject(people));
            });

            this.Get("/actors/{id:int}", async (req, res, routeData) =>
            {
                var person = actorProvider.Get(routeData.Values.AsInt("id"));
                await res.Negotiate(person);
            });

            this.Put("/actors/{id:int}", async (req, res, routeData) =>
            {
                var result = req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                //Update the user in your database

                res.StatusCode = 204;
            });

            this.Post("/actors", async (req, res, routeData) =>
            {
                var result = req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                //Save the user in your database

                res.StatusCode = 201;
                await res.Negotiate(result.Data);
            });
        }
    }
}