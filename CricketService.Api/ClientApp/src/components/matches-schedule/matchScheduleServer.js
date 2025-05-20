const http = require("http");
const fs = require("fs");

const server = http.createServer((req, res) => {
  // Set the content type to HTML
  res.setHeader("Content-Type", "text/html");

  // Handle different URLs
  if (req.url === "/") {
    // Read the HTML file
    fs.readFile("ICCWC2023Fixtures.html", "utf8", (err, data) => {
      if (err) {
        // Handle the error if the file cannot be read
        res.statusCode = 500;
        res.end("<h1>Internal Server Error</h1>");
        return;
      }

      // Send the HTML content as the response
      res.end(data);
    });
  } else {
    // Return a 404 error if the URL is not found
    res.statusCode = 404;
    res.end("<h1>404 Not Found</h1>");
  }
});

// Start the server on port 3000
server.listen(3011, () => {
  console.log("Server is running on http://localhost:3011");
});
