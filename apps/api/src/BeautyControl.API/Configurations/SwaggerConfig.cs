namespace BeautyControl.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
        }

        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
