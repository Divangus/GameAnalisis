<?php
// Database configuration
$servername = "localhost";
$username = "laiapp4";
$password = "wEa6PS8XgQ7b";
$database = "laiapp4";

// Create a database connection
$conn = new mysqli($servername, $username, $password, $database);

// Check the connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Check if the request method is POST
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Read the JSON data from the request body
    $jsonData = $_POST['jsonData'];

    // Decode the JSON data into an associative array
    $userData = json_decode($jsonData, true);

    if ($userData !== null) {
        $name = $userData['name'];
        $age = $userData['age'];
        $country = $userData['country'];
        $gender = $userData['gender'];

        // Prepare an SQL statement to insert the data into the "Users" table
        $sql = "INSERT INTO Users (name, age, country, gender) VALUES (?, ?, ?, ?)";

        // Create a prepared statement
        $stmt = $conn->prepare($sql);

        if ($stmt) {
            // Bind the parameters
            $stmt->bind_param("siss", $name, $age, $country, $gender);

            // Execute the statement
            if ($stmt->execute()) {
                echo "Data inserted into the database successfully.";
            } else {
                echo "Error inserting data: " . $stmt->error;
            }

            // Close the statement
            $stmt->close();
        } else {
            echo "Error preparing statement: " . $conn->error;
        }
    } else {
        echo "Error decoding JSON data.";
        error_log("Received JSON data: " . $jsonData);
    }
} else {
    echo "Invalid request method. Use POST to send data.";
}

// Close the database connection
$conn->close();
?>
