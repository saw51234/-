require('dotenv').config();   // .env 읽기

const express = require('express');
const cors = require('cors');

const db = require('./db/db');       // DB 연결
const authRoutes = require('./routes/auth');   // auth 라우트 추가
const gachaRoutes = require('./routes/gacha');

const app = express();

app.use(cors());
app.use(express.json());

// 서버 살아있는지 테스트용
app.get('/ping', (req, res) => {
  res.json({ success: true, message: 'pong' });
});

// 여기서 auth 라우트를 등록하는 것
app.use('/api/auth', authRoutes);
app.use('/api/gacha', gachaRoutes);

// 포트 설정
const PORT = process.env.PORT || 4000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
