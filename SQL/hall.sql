-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        8.0.43 - MySQL Community Server - GPL
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  12.13.0.7147
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- hall_of_luck 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `hall_of_luck` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `hall_of_luck`;

-- 테이블 hall_of_luck.gacha_results 구조 내보내기
CREATE TABLE IF NOT EXISTS `gacha_results` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '기록 고유 ID',
  `user_id` int NOT NULL COMMENT '유저 ID',
  `item_name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '뽑은 아이템 이름',
  `rank_grade` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '아이템 등급',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '뽑은 시간',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `gacha_results_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 테이블 데이터 hall_of_luck.gacha_results:~3 rows (대략적) 내보내기
DELETE FROM `gacha_results`;
INSERT INTO `gacha_results` (`id`, `user_id`, `item_name`, `rank_grade`, `created_at`) VALUES
	(1, 1, '전설의 검', 'S', '2025-12-02 04:06:54'),
	(2, 1, '튼튼한  방패', 'B', '2025-12-02 04:06:54'),
	(3, 2, '나무 몽둥이', 'C', '2025-12-02 04:06:54'),
	(4, 3, '황금 갑옷', 'A', '2025-12-02 04:06:54');

-- 테이블 hall_of_luck.stats 구조 내보내기
CREATE TABLE IF NOT EXISTS `stats` (
  `user_id` int NOT NULL COMMENT '유저 ID',
  `total_pulls` int DEFAULT '0' COMMENT '총 뽑기 횟수',
  `highest_rank` varchar(10) COLLATE utf8mb4_unicode_ci DEFAULT 'None' COMMENT '뽑은 최고 등급',
  `last_updated` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  CONSTRAINT `stats_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 테이블 데이터 hall_of_luck.stats:~3 rows (대략적) 내보내기
DELETE FROM `stats`;
INSERT INTO `stats` (`user_id`, `total_pulls`, `highest_rank`, `last_updated`) VALUES
	(1, 100, 'S', '2025-12-02 04:06:54'),
	(2, 5, 'B', '2025-12-02 04:06:54'),
	(3, 50, 'A', '2025-12-02 04:06:54');

-- 테이블 hall_of_luck.users 구조 내보내기
CREATE TABLE IF NOT EXISTS `users` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '고유 ID',
  `nickname` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '유저 닉네임',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '가입 일자',
  PRIMARY KEY (`id`),
  UNIQUE KEY `nickname` (`nickname`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 테이블 데이터 hall_of_luck.users:~3 rows (대략적) 내보내기
DELETE FROM `users`;
INSERT INTO `users` (`id`, `nickname`, `created_at`) VALUES
	(1, '고인물유저', '2025-12-02 04:06:54'),
	(2, '뉴비', '2025-12-02 04:06:54'),
	(3, '운빨러', '2025-12-02 04:06:54');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
