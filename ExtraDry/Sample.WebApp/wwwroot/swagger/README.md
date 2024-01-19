### Static Swagger Files
To overload the swagger images: 
  1. place them in this directory
  2. Ensure that `app.UseStaticFiles()` appears before `app.UseSwaggerUI(...)` in Startup.
