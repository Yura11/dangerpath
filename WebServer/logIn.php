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
    
    // Перевірка, чи відправлені дані коректно
    if ($data !== null) {
        // Перевірка наявності електронної адреси або імені користувача
        if (isset($data['logIn'])) {
            // Визначаємо, чи логін є електронною адресою або іменем користувача
            $logIn = $data['logIn'];
            
            // Перевірка, чи логін є електронною адресою
            if (filter_var($logIn, FILTER_VALIDATE_EMAIL)) {
                // Логін є електронною адресою
                $loginField = "email";
            } else {
                // Логін є іменем користувача
                $loginField = "username";
            }
        } else {
            // Логін не включений в запит
            echo "Login is missing in the request.";
            exit; // Зупинити виконання скрипта
        }
        
        // Перевірка наявності паролю
        if (isset($data['password'])) {
            // Пароль включений в запит
            $password = $data['password'];
        } else {
            // Пароль не включений в запит
            echo "Password is missing in the request.";
            exit; // Зупинити виконання скрипта
        }
        
        // Перевірка наявності ідентифікатору пк
        if(isset($data['pcIdent'])) {
            $pcIdent = $data['pcIdent'];    
        } else {
            // Пароль не включений в запит
            echo "PCIdentifier is missing in the request.";
            exit; // Зупинити виконання скрипта
        }
        
        // Підготовка SQL-запиту для перевірки наявності запису з вказаним email або username та перевіреною верифікаційною токеном
        $sql = "SELECT * FROM users WHERE ($loginField = ?) AND verification_token = 'verified'";
        
        // Підготовка та виконання запиту
        $stmt = $conn->prepare($sql);
        $stmt->bind_param("s", $logIn);
        $stmt->execute();
        
        // Отримання результату
        $result = $stmt->get_result();
        
        // Перевірка наявності записів
        if ($result->num_rows > 0) {
            // Якщо знайдено записи з вказаним email або username та верифікованим токеном
            $row = $result->fetch_assoc();
            if (password_verify($password, $row['password_hash'])) {
                // Виконання вставки нового запису
                $sql = "UPDATE users SET pcident = ? WHERE id = ?";
                $stmt = $conn->prepare($sql);
                $stmt->bind_param("si", $pcIdent, $row['id']);
                $stmt->execute();
                // Пароль співпадає, користувач аутентифікований
                $playerUsername = $row['username'];
                echo "User authenticated successfully! PlayerUsename: $playerUsername";
            } else {
                // Пароль не співпадає
                echo "Invalid password.";
            }
        } else {
            // Якщо не знайдено записів з вказаним email або username або токеном не верифіковано
            echo "Invalid login credentials or account not verified.";
        }
        
        // Закриття запиту та з'єднання з базою даних
        $stmt->close();
    } else {
        // Помилка декодування JSON
        echo "Error decoding JSON data.";
    }
} else {
    // POST-запит не містить даних
    echo "No POST data received.";
}

// Закриття з'єднання з базою даних
$conn->close();
?>
