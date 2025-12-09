CREATE DATABASE IF NOT EXISTS hall_of_luck DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE hall_of_luck;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY COMMENT '고유 ID',
    nickname VARCHAR(50) NOT NULL UNIQUE COMMENT '유저 닉네임',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP COMMENT '가입 일자'
);

CREATE TABLE IF NOT EXISTS gacha_results (
    id INT AUTO_INCREMENT PRIMARY KEY COMMENT '기록 고유 ID',
    user_id INT NOT NULL COMMENT '유저 ID',
    item_name VARCHAR(100) NOT NULL COMMENT '뽑은 아이템 이름',
    rank_grade VARCHAR(10) NOT NULL COMMENT '아이템 등급',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP COMMENT '뽑은 시간',
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS stats (
    user_id INT PRIMARY KEY COMMENT '유저 ID',
    total_pulls INT DEFAULT 0 COMMENT '총 뽑기 횟수',
    highest_rank VARCHAR(10) DEFAULT 'None' COMMENT '뽑은 최고 등급',
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

SHOW TABLES;



INSERT INTO users (nickname) VALUES ('고인물유저'), ('뉴비'), ('운빨러');

INSERT INTO stats (user_id, total_pulls, highest_rank) VALUES 
(1, 100, 'S'), 
(2, 5, 'B'), 
(3, 50, 'A');

INSERT INTO gacha_results (user_id, item_name, rank_grade) VALUES 
(1, '전설의 검', 'S'),
(1, '튼튼한  방패', 'B'),
(2, '나무 몽둥이', 'C'),
(3, '황금 갑옷', 'A');

SELECT * FROM users;
SELECT * FROM stats;


CREATE USER IF NOT EXISTS 'game_admin'@'localhost' IDENTIFIED BY '1234';
GRANT ALL PRIVILEGES ON hall_of_luck.* TO 'game_admin'@'localhost';
FLUSH PRIVILEGES;