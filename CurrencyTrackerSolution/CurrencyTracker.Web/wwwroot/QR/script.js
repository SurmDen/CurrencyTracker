const today = new Date();
const day = String(today.getDate()).padStart(2, '0');
const month = String(today.getMonth() + 1).padStart(2, '0');
const year = today.getFullYear();
const dateStr = `${day}/${month}/${year}`;
const url = `https://www.cbr.ru/scripts/XML_daily.asp?date_req=${dateStr}`;

document.getElementById('url').textContent = url;

const qrGenerator = qrcode(0, 'M');
qrGenerator.addData(url);
qrGenerator.make();

document.getElementById('qrcode').innerHTML = qrGenerator.createImgTag(4);