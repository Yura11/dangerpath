const express = require('express');
const mysql = require('mysql');
const nodemailer = require('nodemailer');
const fs = require('fs');

const app = express();
app.use(express.json());

// Зчитування даних з JSON файлу
const configData = JSON.parse(fs.readFileSync('nodeServerConfig.json', 'utf-8'));
const dbPassword = configData.dbPassword;
const emailUsername = configData.emailUsername;
const emailPassword = configData.emailPassword;

const connection = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: dbPassword,
  database: 'dangerpath'
});

connection.connect((err) => {
  if (err) throw err;
  console.log('Connected to MySQL database!');
});

// Генерування токену для верифікації
function generateVerificationToken() {
  // Ваш код генерації токену
}

// Відправлення листа з підтвердженням
function sendVerificationEmail(email, verificationToken) {
  // Конфігурація транспортера електронної пошти
  const transporter = nodemailer.createTransport({
    service: 'gmail',
    port: 465,
    secure: true,
    logger: true,
    debug: true,
    secureConnection: false,
      auth: {
      user: emailUsername, // Використовуємо дані з JSON файлу
      pass: emailPassword // Використовуємо дані з JSON файлу
    },
    tls:{
      rejectUnauthorized: true
    }
  });

  // Параметри листа
  const mailOptions = {
    from: emailUsername,
    to: email,
    subject: 'Підтвердження реєстрації',
    text: `Для підтвердження реєстрації перейдіть за посиланням: http://localhost:9099/dangerpath/register`
  };

  // Надсилання листа
  transporter.sendMail(mailOptions, function(error, info) {
    if (error) {
      console.log(error);
    } else {
      console.log('Email sent: ' + info.response);
    }
  });
}

// Реєстрація нового маршруту для підтвердження електронної пошти
app.post('/verification', (req, res) => {
  const { email, verificationToken } = req.body;
  sendVerificationEmail(email, verificationToken);
  // Ваш код для підтвердження електронної пошти тут
});

// Реєстрація нового маршруту для реєстрації користувачів
app.post('/register', (req, res) => {
  const { username, email, password } = req.body;
  
  const sql = 'INSERT INTO users (username, email, password_hash, verification_token) VALUES (?, ?, ?, ?)';
  const verificationToken = generateVerificationToken();
  connection.query(sql, [username, email, password, verificationToken], (err, result) => {
      if (err) {
          console.error('Error registering user:', err);
          res.status(500).json({ error: 'Internal server error' });
      } else {
          console.log('User registered successfully!');
          
          res.status(200).json({ message: 'User registered successfully' });
      }
  });
});

app.listen(3000, () => {
  console.log('Server is running on http://localhost:3000');
});
