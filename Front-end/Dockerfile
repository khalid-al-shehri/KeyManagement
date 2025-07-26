# Use Python to serve static files
FROM python:3-alpine

# Set working directory
WORKDIR /app

# Copy static files
COPY . .

# Expose port 80
EXPOSE 80

# Start Python HTTP server
CMD ["python", "-m", "http.server", "80"]
