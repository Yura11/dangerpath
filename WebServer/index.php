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
  echo "Не вдалося підключитися до бази даних: " . $conn->connect_error;
} else {
  echo "Підключення до бази даних успішне!";
}
?>
