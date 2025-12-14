const express = require('express');
const router = express.Router();
const pool = require('../db/db');

// [아이템 목록 및 확률 설정]
// 총 15개 아이템
// 확률 합계: 0.01 + 0.04 + 0.15 + 0.30 + 0.50 = 1.0 (100%)
const items = [
    // [SS등급: 1개] (1%)
    { name: "SS급 전설의 성배", rank: "SS", rate: 0.01 },

    // [S등급: 2개] (개당 2% -> 합 4%)
    { name: "S급 드래곤 슬레이어", rank: "S", rate: 0.02 },
    { name: "S급 천사의 지팡이", rank: "S", rate: 0.02 },

    // [A등급: 3개] (개당 5% -> 합 15%)
    { name: "A급 미스릴 갑옷", rank: "A", rate: 0.05 },
    { name: "A급 엘프의 활", rank: "A", rate: 0.05 },
    { name: "A급 황금 사과", rank: "A", rate: 0.05 },

    // [B등급: 4개] (개당 7.5% -> 합 30%)
    { name: "B급 강철 검", rank: "B", rate: 0.075 },
    { name: "B급 튼튼한 방패", rank: "B", rate: 0.075 },
    { name: "B급 가죽 장화", rank: "B", rate: 0.075 },
    { name: "B급 마법 물약", rank: "B", rate: 0.075 },

    // [C등급: 5개] (개당 10% -> 합 50%)
    { name: "C급 나무 몽둥이", rank: "C", rate: 0.10 },
    { name: "C급 낡은 망토", rank: "C", rate: 0.10 },
    { name: "C급 녹슨 단검", rank: "C", rate: 0.10 },
    { name: "C급 돌멩이", rank: "C", rate: 0.10 },
    { name: "C급 빈 물병", rank: "C", rate: 0.10 }
];


function rollGacha() {
    let rand = Math.random();
    for (let item of items) {
        if (rand < item.rate) return item;
        rand -= item.rate;
    }
    return items[items.length - 1];
}


router.post('/pull', async (req, res) => {
    const { userId } = req.body;

    if (!userId) {
        return res.json({ success: false, message: "userId 필요" });
    }

    try {
        const result = rollGacha();

        await pool.query(
            "INSERT INTO gacha_results (user_id, item_name, rank_grade) VALUES (?, ?, ?)",
            [userId, result.name, result.rank]
        );


        await pool.query(
            "UPDATE stats SET total_pulls = total_pulls + 1 WHERE user_id = ?",
            [userId]
        );


        await pool.query(
            `UPDATE stats 
             SET highest_rank = ? 
             WHERE user_id = ? 
             AND FIELD(highest_rank, 'SS', 'S', 'A', 'B', 'C') > FIELD(?, 'SS', 'S', 'A', 'B', 'C')`,
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

router.get('/ranking', async (req, res) => {
    try {

        const [rows] = await pool.query(`
            SELECT u.nickname, s.total_pulls, s.highest_rank
            FROM stats s 
            JOIN users u ON s.user_id = u.id 
            ORDER BY FIELD(s.highest_rank, 'SS', 'S', 'A', 'B', 'C') ASC, s.total_pulls DESC 
            LIMIT 20
        `);

        res.json({ success: true, data: rows });

    } catch (err) {
        console.error(err);
        res.json({ success: false, message: "서버 오류 발생" });
    }
});

module.exports = router;