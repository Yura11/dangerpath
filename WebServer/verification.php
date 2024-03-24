<?php
header("Access-Control-Allow-Origin: *");

// Зчитування параметрів підключення з JSON-файлу
$config = json_decode(file_get_contents('config.json'), true);

// Параметри підключення до бази даних
$servername = $config['servername'];
$username = $config['username'];
$password = $config['password'];
$dbname = $config['dbname'];

// Створення з'єднання з базою даних
$conn = new mysqli($servername, $username, $password, $dbname);

// Перевірка з'єднання
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Отримання токену з параметру запиту
$token = $_GET['token'] ?? null;

// Перевірка, чи був переданий токен
if ($token !== null) {
    // Підготовлений запит для оновлення поля verification_token на "verified"
    $sql = "UPDATE users SET verification_token = 'verified' WHERE verification_token = ?";
    $stmt = $conn->prepare($sql);
    
    // Прив'язка параметрів
    $stmt->bind_param("s", $token);
    
    // Виконання запиту
    if ($stmt->execute()) {
        echo json_encode(array("success" => true, "message" => "User verified successfully"));
    } else {
        echo json_encode(array("success" => false, "message" => "Error verifying user: " . $conn->error));
    }

    // Закриття підготовленого запиту і з'єднання
    $stmt->close();
} else {
    echo json_encode(array("success" => false, "message" => "No token provided"));
}

// Закриття з'єднання
$conn->close();
?>
