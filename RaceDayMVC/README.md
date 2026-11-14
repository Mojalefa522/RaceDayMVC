## Docker Setup

To build and run the MVC application inside a Docker container:

1. Ensure Docker Desktop is running.
2. Open a terminal in the `RaceDayMVC` folder.
3. Run the following commands:

```bash
docker build -t racedaymvc .
docker run -d -p 8080:8080 --name racedaymvc-container racedaymvc