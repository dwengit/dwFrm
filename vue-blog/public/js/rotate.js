window.tagcloud = (function(win, doc) {
  function isObject(obj) {
    return Object.prototype.toString.call(obj) === "[object Object]";
  }
  function TagCloud(options) {
    var self = this;
    self.config = TagCloud._getConfig(options);
    self.box = self.config.element;
    self.fontsize = self.config.fontsize;
    self.radius = self.config.radius;
    self.depth = 2 * self.radius;
    self.size = 2 * self.radius;
    self.mspeed = TagCloud._getMsSpeed(self.config.mspeed);
    self.ispeed = TagCloud._getIsSpeed(self.config.ispeed);
    self.items = self._getItems();
    self.direction = self.config.direction;
    self.keep = self.config.keep;
    self.active = false;
    self.lasta = 1;
    self.lastb = 1;
    self.mouseX0 =
      self.ispeed * Math.sin((self.direction * Math.PI) / 180);
    self.mouseY0 =
      -self.ispeed * Math.cos((self.direction * Math.PI) / 180);
    self.mouseX = self.mouseX0;
    self.mouseY = self.mouseY0;
    self.index = -1;
    TagCloud._on(self.box, "mouseover", function() {
      self.active = true;
    });
    TagCloud._on(self.box, "mouseout", function() {
      self.active = false;
    });
    TagCloud._on(self.keep ? win : self.box, "mousemove", function(ev) {
      var oEvent = win.event || ev;
      var boxPosition = self.box.getBoundingClientRect();
      self.mouseX =
        (oEvent.clientX - (boxPosition.left + self.box.offsetWidth / 2)) /
        5;
      self.mouseY =
        (oEvent.clientY - (boxPosition.top + self.box.offsetHeight / 2)) /
        5;
    });
    for (var j = 0, len = self.items.length; j < len; j++) {
      self.items[j].element.index = j;
      self.items[j].element.onmouseover = function() {
        self.index = this.index;
      };
      self.items[j].element.onmouseout = function() {
        self.index = -1;
      };
    }
    TagCloud.boxs.push(self.box);
    self.update(self);
    self.box.style.visibility = "visible";
    self.box.style.position = "relative";
    self.box.style.minHeight = 1.2 * self.size + "px";
    self.box.style.minWidth = 2.5 * self.size + "px";
    for (var j = 0, len = self.items.length; j < len; j++) {
      self.items[j].element.style.position = "absolute";
      self.items[j].element.style.zIndex = j + 1;
    }
    self.up = setInterval(function() {
      self.update(self);
    }, 30);
  }
  TagCloud.boxs = [];
  TagCloud._set = function(element) {
    if (TagCloud.boxs.indexOf(element) == -1) {
      return true;
    }
  };
  if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function(elt) {
      var len = this.length >>> 0;
      var from = Number(arguments[1]) || 0;
      from = from < 0 ? Math.ceil(from) : Math.floor(from);
      if (from < 0) from += len;
      for (; from < len; from++) {
        if (from in this && this[from] === elt) return from;
      }
      return -1;
    };
  }
  TagCloud._getConfig = function(config) {
    var defaultConfig = {
      fontsize: 16,
      radius: 60,
      mspeed: "normal",
      ispeed: "normal",
      direction: 135,
      keep: true,
    };
    if (isObject(config)) {
      for (var i in config) {
        if (config.hasOwnProperty(i)) {
          defaultConfig[i] = config[i];
        }
      }
    }
    return defaultConfig;
  };
  TagCloud._getMsSpeed = function(mspeed) {
    var speedMap = {
      slow: 1.5,
      normal: 3,
      fast: 5,
    };
    return speedMap[mspeed] || 3;
  };
  TagCloud._getIsSpeed = function(ispeed) {
    var speedMap = {
      slow: 10,
      normal: 25,
      fast: 50,
    };
    return speedMap[ispeed] || 25;
  };
  TagCloud._getSc = function(a, b) {
    var l = Math.PI / 180;
    return [
      Math.sin(a * l),
      Math.cos(a * l),
      Math.sin(b * l),
      Math.cos(b * l),
    ];
  };
  TagCloud._on = function(ele, eve, handler, cap) {
    if (ele.addEventListener) {
      ele.addEventListener(eve, handler, cap);
    } else if (ele.attachEvent) {
      ele.attachEvent("on" + eve, handler);
    } else {
      ele["on" + eve] = handler;
    }
  };
  TagCloud.prototype = {
    constructor: TagCloud,
    update: function() {
      var self = this,
        a,
        b;
      if (!self.active && !self.keep) {
        self.mouseX =
          Math.abs(self.mouseX - self.mouseX0) < 1
            ? self.mouseX0
            : (self.mouseX + self.mouseX0) / 2;
        self.mouseY =
          Math.abs(self.mouseY - self.mouseY0) < 1
            ? self.mouseY0
            : (self.mouseY + self.mouseY0) / 2;
      }
      a =
        -(
          Math.min(Math.max(-self.mouseY, -self.size), self.size) /
          self.radius
        ) * self.mspeed;
      b =
        (Math.min(Math.max(-self.mouseX, -self.size), self.size) /
          self.radius) *
        self.mspeed;
      if (Math.abs(a) <= 0.01 && Math.abs(b) <= 0.01) {
        return;
      }
      self.lasta = a;
      self.lastb = b;
      var sc = TagCloud._getSc(a, b);
      for (var j = 0, len = self.items.length; j < len; j++) {
        var rx1 = self.items[j].x,
          ry1 = self.items[j].y * sc[1] + self.items[j].z * -sc[0],
          rz1 = self.items[j].y * sc[0] + self.items[j].z * sc[1];
        var rx2 = rx1 * sc[3] + rz1 * sc[2],
          ry2 = ry1,
          rz2 = rz1 * sc[3] - rx1 * sc[2];
        if (self.index == j) {
          self.items[j].scale = 1;
          self.items[j].fontsize = 16;
          self.items[j].alpha = 1;
          self.items[j].element.style.zIndex = 99;
        } else {
          var per = self.depth / (self.depth + rz2);
          self.items[j].x = rx2;
          self.items[j].y = ry2;
          self.items[j].z = rz2;
          self.items[j].scale = per;
          self.items[j].fontsize = Math.ceil(per * 2) + self.fontsize - 6;
          self.items[j].alpha = 1.5 * per - 0.5;
          self.items[j].element.style.zIndex = Math.ceil(per * 10 - 5);
        }
        self.items[j].element.style.fontSize =
          self.items[j].fontsize + "px";
        self.items[j].element.style.left =
          self.items[j].x +
          (self.box.offsetWidth - self.items[j].offsetWidth) / 2 +
          "px";
        self.items[j].element.style.top =
          self.items[j].y +
          (self.box.offsetHeight - self.items[j].offsetHeight) / 2 +
          "px";
        self.items[j].element.style.filter =
          "alpha(opacity=" + 100 * self.items[j].alpha + ")";
        self.items[j].element.style.opacity = self.items[j].alpha;
      }
    },
    _getItems: function() {
      var self = this,
        items = [],
        element = self.box.children,
        length = element.length,
        item;
      for (var i = 0; i < length; i++) {
        item = {};
        item.angle = {};
        item.angle.phi = Math.acos(-1 + (2 * i + 1) / length);
        item.angle.theta =
          Math.sqrt((length + 1) * Math.PI) * item.angle.phi;
        item.element = element[i];
        item.offsetWidth = item.element.offsetWidth;
        item.offsetHeight = item.element.offsetHeight;
        item.x =
          self.radius *
          1.5 *
          Math.cos(item.angle.theta) *
          Math.sin(item.angle.phi);
        item.y =
          self.radius *
          1.5 *
          Math.sin(item.angle.theta) *
          Math.sin(item.angle.phi);
        item.z = self.radius * 1.5 * Math.cos(item.angle.phi);
        item.element.style.left =
          item.x + (self.box.offsetWidth - item.offsetWidth) / 2 + "px";
        item.element.style.top =
          item.y + (self.box.offsetHeight - item.offsetHeight) / 2 + "px";
        items.push(item);
      }
      return items;
    },
  };
  if (!doc.querySelectorAll) {
    doc.querySelectorAll = function(selectors) {
      var style = doc.createElement("style"),
        elements = [],
        element;
      doc.documentElement.firstChild.appendChild(style);
      doc._qsa = [];
      style.styleSheet.cssText =
        selectors +
        "{x-qsa:expression(document._qsa && document._qsa.push(this))}";
      window.scrollBy(0, 0);
      style.parentNode.removeChild(style);
      while (doc._qsa.length) {
        element = doc._qsa.shift();
        element.style.removeAttribute("x-qsa");
        elements.push(element);
      }
      doc._qsa = null;
      return elements;
    };
  }
  return function(options) {
    options = options || {};
    var selector = options.selector || ".tagcloud",
      elements = doc.querySelectorAll(selector),
      instance = [];
    for (var index = 0, len = elements.length; index < len; index++) {
      options.element = elements[index];
      if (!!TagCloud._set(options.element)) {
        instance.push(new TagCloud(options));
      }
    }
    return instance;
  };
})(window, document);