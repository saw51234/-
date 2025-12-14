const express = require('express');
const router = express.Router();
const pool = require('../db/db');

// 아이템 확률 설정
const items = [
    { name: "S급 전설검", rank: "S", rate: 0.05 },
    { name: "A급 활", rank: "A", rate: 0.25 },
    { name: "B급 단검", rank: "B", rate: 0.70 }
];

function rollGacha() {
    let rand = Math.random();
    for (let item of items) {
        if (rand < item.rate) return item;
        rand -= item.rate;
    }
    return items[items.length - 1];
}

// 1) 가챠 1회 API
router.post('/pull', async (req, res) => {
    const { userId } = req.body;

    if (!userId) {
        return res.json({ success: false, message: "userId 필요" });
    }

    try {
        const result = rollGacha();

        // 1. 뽑기 결과 기록 (gacha_results)
        await pool.query(
            "INSERT INTO gacha_results (user_id, item_name, rank_grade) VALUES (?, ?, ?)",
            [userId, result.name, result.rank]
        );

        // 2. 총 뽑기 횟수 증가 (stats)
        await pool.query(
            "UPDATE stats SET total_pulls = total_pulls + 1 WHERE user_id = ?",
            [userId]
        );

        await pool.query(
            `UPDATE stats 
             SET highest_rank = ? 
             WHERE user_id = ? 
             AND FIELD(highest_rank, 'S', 'A', 'B') > FIELD(?, 'S', 'A', 'B')`,
            [result.rank, userId, result.rank]
        );

        res.json({
            success: true,
            message: "가챠 성공",
            data: result
        });

    } catch (err) {
        console.error(err);
        res.json({ success: false, message: "서버 오류 발생" });
    }
});

// 2) 최근 10개 기록 조회
router.get('/history/:userId', async (req, res) => {
    const { userId } = req.params;

    try {
        const [rows] = await pool.query(
            "SELECT item_name, rank_grade, created_at FROM gacha_results WHERE user_id = ? ORDER BY created_at DESC LIMIT 10",
            [userId]
        );

        res.json({ success: true, data: rows });
    } catch (err) {
        console.error(err);
        res.json({ success: false, message: "서버 오류 발생" });
    }
});

// 3) 랭킹 조회
router.get('/ranking', async (req, res) => {
    try {
        const [rows] = await pool.query(`
            SELECT u.nickname, s.total_pulls, s.highest_rank
            FROM stats s 
            JOIN users u ON s.user_id = u.id 
            ORDER BY FIELD(s.highest_rank, 'S', 'A', 'B') ASC, s.total_pulls DESC 
            LIMIT 20
        `);

        res.json({ success: true, data: rows });

    } catch (err) {
        console.error(err);
        res.json({ success: false, message: "서버 오류 발생" });
    }
});

module.exports = router;