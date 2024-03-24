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
    
    // Перевірка наявності ідентифікатору пк
    if(isset($data['pcIdent'])) {
        $pcIdent = $data['pcIdent'];    
    } else {
        // Пароль не включений в запит
        echo "PCIdentifier is missing in the request.";
        exit; // Зупинити виконання скрипта
    }

    $sql = "SELECT username FROM users WHERE pcident = ? AND verification_token = 'verified'";

    // Підготовка та виконання запиту
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("s", $pcIdent);
    $stmt->execute();

    // Отримання результату
    $result = $stmt->get_result();

    // Перевірка наявності записів
    if ($result->num_rows > 0) {
        // Якщо знайдено записи з вказаним pcIdent та верифікованим токеном
        $row = $result->fetch_assoc();
        $username = $row['username'];
        echo "Username: $username";
    } else {
        echo "Not logged in";
    }

    // Закриття запиту та з'єднання з базою даних
    $stmt->close();
} else {
    // POST-запит не містить даних
    echo "No POST data received.";
}

$conn->close();
?>
