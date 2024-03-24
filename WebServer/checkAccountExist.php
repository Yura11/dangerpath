<?php
header("Access-Control-Allow-Origin: *");

// Отримання всього POST-запиту
$postData = file_get_contents('php://input');

// Зчитування параметрів підключення з JSON-файлу
$config = json_decode(file_get_contents('config.json'), true);

// Параметри підключення до бази даних
$servername = $config['servername'];
$usernameDB = $config['username']; // Використовуємо іншу змінну, наприклад, $usernameDB
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
    
    // Перевірка, чи відправлені дані коректно
    if ($data !== null) {
        // Перевірка наявності електронної адреси
        if (isset($data['email'])) {
            // Електронна адреса включена в запит
            $email = $conn->real_escape_string($data['email']);
        } 
        
        // Перевірка наявності імені користувача
        if (isset($data['username'])) {
            // Ім'я користувача включено в запит
            $username = $conn->real_escape_string($data['username']);
        } 
    } else {
        // Помилка декодування JSON
        echo "Error decoding JSON data.";
    }
} else {
    // POST-запит не містить даних
    echo "No POST data received.";
}

// Підготовка SQL-запиту для перевірки наявності запису з вказаним email або username та перевіреною верифікаційною токеном
$sql = "SELECT * FROM users WHERE (email = ? OR username = ?) AND verification_token = 'verified'";
$stmt = $conn->prepare($sql);

// Параметризація SQL-запиту
$stmt->bind_param("ss", $email, $username);

// Виконання запиту
$stmt->execute();

// Отримання результату
$stmt->store_result();

// Перевірка наявності записів
if ($stmt->num_rows > 0) {
    // Якщо знайдено записи з вказаним email або username та верифікованим токеном
    echo "exists";
} else {
    // Якщо не знайдено записів з вказаним email або username або токеном не верифіковано
    echo "not_exists";
}

// Закриття з'єднання з базою даних
$stmt->close();
$conn->close();
?>