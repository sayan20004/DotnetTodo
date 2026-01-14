# Use ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy everything
COPY . .

# Build & publish
RUN dotnet publish -c Release -o out

# Render uses port 8080 internally
EXPOSE 8080

# Start app
CMD ["dotnet", "out/TodoDotNet.dll"]