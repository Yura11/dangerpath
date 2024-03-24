<?php
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

// Підготовка SQL-запиту для вставки даних
$sql = "INSERT INTO users (username, password_hash, email) VALUES ('test', '" . password_hash('test', PASSWORD_DEFAULT) . "', 'test@test.com')";

// Виконання SQL-запиту
if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

// Закриття з'єднання з базою даних
$conn->close();
?>
