const express = require('express');
const router = express.Router();
const pool = require('../db/db');

// 닉네임 로그인 & 자동 회원가입
router.post('/login', async (req, res) => {
    const { nickname } = req.body;

    if (!nickname) {
        return res.json({
            success: false,
            message: "닉네임이 필요합니다."
        });
    }

    try {
        // 1) 유저 존재 확인
        const [rows] = await pool.query(
            "SELECT id, nickname FROM users WHERE nickname = ?",
            [nickname]
        );

        // 2) 유저 없으면 생성
        if (rows.length === 0) {
            const [insertUser] = await pool.query(
                "INSERT INTO users (nickname) VALUES (?)",
                [nickname]
            );

            const newUserId = insertUser.insertId;

            // stats 초기화
            await pool.query(
                "INSERT INTO stats (user_id, total_pulls, highest_rank) VALUES (?, 0, 'None')",
                [newUserId]
            );

            return res.json({
                success: true,
                message: "새 유저 생성 + 로그인 성공",
                data: {
                    userId: newUserId,
                    nickname: nickname
                }
            });
        }

        // 3) 기존 유저 로그인
        return res.json({
            success: true,
            message: "로그인 성공",
            data: {
                userId: rows[0].id,
                nickname: rows[0].nickname
            }
        });

    } catch (err) {
        console.error(err);
        return res.json({
            success: false,
            message: "서버 오류 발생"
        });
    }
});

module.exports = router;
