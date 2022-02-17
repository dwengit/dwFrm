export const isPC = (() => {
  let userAgentInfo = navigator.userAgent;
  let Agents = [
    'Android',
    'iPhone',
    'SymbianOS',
    'Windows Phone',
    'iPad',
    'iPod',
    'XiaoMi/MiuiBrowser'
  ];
  let pc = true;
  for (let v = 0; v < Agents.length; v++) {
    if (userAgentInfo.indexOf(Agents[v]) > 0) {
      pc = false;
      break;
    }
  }
  return pc && window.innerWidth > 750;
})();

export const colors = [
  '#f44336',
  '#e91e63',
  '#9c27b0',
  '#673ab7',
  '#3f51b5',
  '#2196f3',
  '#03a9f4',
  '#00bcd4',
  '#009688',
  '#4caf50',
  '#8bc34a',
  '#ff5722',
  '#795548',
  '#607d8b'
];

export const randomNum = (m, n) => {
  return Math.floor(Math.random() * (m - n) + n);
};

export const randomColor = () => {
  return colors[randomNum(1, 15)];
};
export const emojis = [
  '😀', '😄', '😅', '🤣', '😂', '😉', '😊', '😍', '😘', '😜',
  '😝', '😏', '😒', '🙄', '😔', '😴', '😷', '🤮', '🥵', '😎',
  '😮', '😰', '😭', '😱', '😩', '😡', '💀', '👽', '🤓', '🥳',
  '😺', '😹', '😻', '🤚', '💩', '👍', '👎', '👏', '👌', '💪'
];
export const VALIDATE_KEY = 'validateKey';

export const getGuid = () => {
  const guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
    /[xy]/g,
    function(c) {
      const r = (Math.random() * 16) | 0;
      const v = c === 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    }
  );
  return guid;
};
export const guidEmpty = '00000000-0000-0000-0000-000000000000';
