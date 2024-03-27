<?php
header("Access-Control-Allow-Origin: *");

// Отримання всього POST-запиту
$postData = file_get_contents('php://input');

// Зчитування параметрів підключення з JSON-файлу
$config = json_decode(file_get_contents('config.json'), true);

// Параметри підключення до бази даних
$servername = $config['servername'];
$usernameDB = $config['username']; 
$password = $config['password'];
$dbname = $config['dbname'];

// Створення з'єднання з базою даних
$conn = new mysqli($servername, $usernameDB, $password, $dbname);

// Перевірка з'єднання
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Отримання даних з POST-запиту та очищення їх від потенційно шкідливих символів
if (isset($postData)) {
    // Декодування JSON даних
    $data = json_decode($postData, true);
    if ($data !== null) {
        if(isset($data['pcIdent'])) {
            $pcIdent = $data['pcIdent'];    
        } else {
            // Пароль не включений в запит
            echo "PCIdentifier is missing in the request.";
            exit; // Зупинити виконання скрипта
        }
        
        // Підготовка SQL-запиту для перевірки наявності запису з вказаним email або username та перевіреною верифікаційною токеном
        $sql = "SELECT * FROM users WHERE ($pcIdent = ?) AND verification_token = 'verified'";
        
        // Підготовка та виконання запиту
            // Підготовка SQL-запиту для зміни значення поля pcIdent на NULL
            $sql = "UPDATE users SET pcIdent = NULL WHERE pcIdent = ?";
            
            // Підготовка та виконання запиту
            $stmt = $conn->prepare($sql);
            $stmt->bind_param("s", $pcIdent);
            $stmt->execute();
            echo "logged out!";
        }
    }
$conn->close();
?>
