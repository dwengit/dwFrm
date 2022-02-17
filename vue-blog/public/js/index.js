function App(ref_canvas) {
  const conf = {
    el: ref_canvas,
    fov: 50,
    cameraZ: 400 };


  const { WebGLRenderer, PerspectiveCamera, OrbitControls, AmbientLight, DirectionalLight, Scene } = THREE;
  const { Object3D, CylinderGeometry, IcosahedronGeometry, SphereGeometry, MeshLambertMaterial, Mesh, Vector3 } = THREE;
  const { randFloat: rnd, randFloatSpread: rndFS } = THREE.Math;
  const { random, PI } = Math;
  const simplex = new SimplexNoise();

  let renderer, scene, camera, cameraCtrl;
  let width, height;
  let planet;

  init();

  function init() {
    renderer = new WebGLRenderer({ canvas: document.getElementById(conf.el), antialias: true, alpha: true });
    camera = new PerspectiveCamera(conf.fov);
    camera.position.z = conf.cameraZ;
    cameraCtrl = new OrbitControls(camera, renderer.domElement);
    cameraCtrl.enableDamping = true;
    cameraCtrl.dampingFactor = 0.1;
    cameraCtrl.rotateSpeed = 0.1;
    cameraCtrl.autoRotate = true;
    cameraCtrl.autoRotateSpeed = 0.1;

    updateSize();
    window.addEventListener('resize', updateSize, false);

    initScene();
    animate();
  }

  function initScene() {
    scene = new Scene();
    scene.add(new AmbientLight(0xcccccc));

    const light = new DirectionalLight(0xffffff);
    light.position.x = 200;
    light.position.z = 100;
    scene.add(light);

    // planet
    planet = new Object3D();
    scene.add(planet);

    // noise buffer for faces colors
    const noises = [];

    // noise conf
    const noiseF = 0.015;
    const noiseD = 15;
    const noiseWaterTreshold = 0.4;
    const noiseWaterLevel = 0.2;

    // noise function
    const vNoise = (v, f, i) => {
      const nv = new Vector3(v.x, v.y, v.z).multiplyScalar(f);
      let noise = (simplex.noise3D(nv.x, nv.y, nv.z) + 1) / 2;
      noise = noise > noiseWaterTreshold ? noise : noiseWaterLevel;
      if (Number.isInteger(i)) noises[i] = noise;
      return noise;
    };

    // displacement function
    const dispV = (v, i) => {
      const dv = new Vector3(v.x, v.y, v.z);
      dv.add(dv.clone().normalize().multiplyScalar(vNoise(dv, noiseF, i) * noiseD));
      v.x = dv.x;v.y = dv.y;v.z = dv.z;
    };

    // planet geometry
    let geometry, material, mesh;
    geometry = new IcosahedronGeometry(100, 4);
    for (let i = 0; i < geometry.vertices.length; i++) dispV(geometry.vertices[i], i);
    geometry.computeFlatVertexNormals();

    // planet geometry - faces colors
    for (let i = 0; i < geometry.faces.length; i++) {
      let f = geometry.faces[i];
      f.color.setHex(0x417B2B);
      if (noises[f.a] == noiseWaterLevel && noises[f.b] == noiseWaterLevel && noises[f.c] == noiseWaterLevel) {
        f.color.setHex(0x2080D0);
      }
    }

    // planet mesh
    material = new MeshLambertMaterial({ flatShading: true, vertexColors: THREE.VertexColors });
    mesh = new Mesh(geometry, material);
    planet.add(mesh);

    // start anim
    planet.scale.set(0.3, 0.3, 0.3);
    TweenMax.to(planet.scale, rnd(2, 5), { x: 1, y: 1, z: 1, ease: Power1.easeOut });

    // add trees & rocks
    objects = [];
    const cscale = chroma.scale([0x509A36, 0xFF5A36, 0x509A36, 0xFFC236, 0x509A36]);
    const points = getFibonacciSpherePoints(800, 100);
    let p, obj;
    for (let i = 0; i < points.length; i++) {
      p = points[i];
      dispV(p);
      if (vNoise(p, noiseF) == noiseWaterLevel) continue;
      if (random() > 0.3) {
        const tsize = rnd(5, 15);
        const bsize = tsize * rnd(0.5, 0.7);
        const vn2 = vNoise(p, 0.01);
        obj = createTree(tsize, bsize, 0x764114, cscale(vn2).hex());
        obj.position.set(p.x, p.y, p.z);
        obj.lookAt(0, 0, 0);
      } else {
        obj = createRock(rnd(2, 4));
        obj.position.set(p.x, p.y, p.z);
      }
      objects.push(obj);
      obj.scale.set(0.01, 0.01, 0.01);
      obj.tween = TweenMax.to(obj.scale, rnd(3, 10), { x: 1, y: 1, z: 1, ease: Elastic.easeOut.config(1, 0.2), delay: rnd(0, 4) });
      planet.add(obj);
    }

    // interactivity
    const mouse = new THREE.Vector2();
    const raycaster = new THREE.Raycaster();
    const onMouseMove = e => {
      mouse.x = e.clientX / width * 2 - 1;
      mouse.y = -(e.clientY / height) * 2 + 1;
      raycaster.setFromCamera(mouse, camera);
      const intersects = raycaster.intersectObjects(objects, true);
      if (intersects.length > 0) {
        let obj = intersects[0].object;
        obj = obj.tween ? obj : obj.parent;
        if (!obj.tween.isActive()) {
          obj.scale.set(0.5, 0.5, 0.5);
          obj.tween = TweenMax.to(obj.scale, 1.5, { x: 1, y: 1, z: 1, ease: Elastic.easeOut.config(1, 0.2) });
        }
      }
    };
    renderer.domElement.addEventListener('mousemove', onMouseMove);
  }

  // low poly tree
  function createTree(tsize, bsize, tcolor, bcolor) {
    const tradius = tsize * 0.1;
    const t1size = tsize / 2,t1radius = tradius * 0.7;

    const tmaterial = new MeshLambertMaterial({ color: tcolor, flatShading: true });
    const bmaterial = new MeshLambertMaterial({ color: bcolor, flatShading: true });

    const tree = new Object3D();

    // trunk
    const tgeometry = new CylinderGeometry(tradius * 0.7, tradius, tsize, 5, 3, true);
    tgeometry.translate(0, tsize / 2, 0);
    tgeometry.rotateX(-PI / 2);
    rdnGeo(tgeometry, tradius * 0.2);
    const tmesh = new Mesh(tgeometry, tmaterial);
    tree.add(tmesh);

    // body
    const bgeometry = new SphereGeometry(bsize, 4, 4);
    bgeometry.translate(0, tsize + bsize * 0.7, 0);
    bgeometry.rotateX(-PI / 2);
    rdnGeo(bgeometry, bsize * 0.2);
    const bmesh = new Mesh(bgeometry, bmaterial);
    tree.add(bmesh);

    if (random() > 0.5) {
      // trunk 1
      const t1geometry = new CylinderGeometry(t1radius * 0.5, t1radius, t1size, 4, 2, true);
      t1geometry.translate(0, t1size / 2, 0);
      t1geometry.rotateZ(PI / 3 + rnd(0, 0.2));
      t1geometry.rotateY(rndFS(PI / 2));
      t1geometry.translate(0, tsize * rnd(0.2, 0.7), 0);
      t1geometry.rotateX(-PI / 2);
      rdnGeo(t1geometry, tradius * 0.1);
      tgeometry.merge(t1geometry);

      // body 1
      const b1size = bsize * rnd(0.5, 0.8);
      const t1bp = getTrunkBodyPosition(t1geometry, b1size);
      const b1geometry = new SphereGeometry(b1size, 4, 4);
      b1geometry.translate(t1bp.x, t1bp.y, t1bp.z);
      rdnGeo(b1geometry, b1size * 0.2);
      bgeometry.merge(b1geometry);
    }

    if (random() > 0.5) {
      // trunk 2
      const t2geometry = new CylinderGeometry(t1radius * 0.5, t1radius, t1size, 4, 2, true);
      t2geometry.translate(0, t1size / 2, 0);
      t2geometry.rotateZ(-PI / 3 + rnd(0, 0.2));
      t2geometry.rotateY(rndFS(PI / 2));
      t2geometry.translate(0, tsize * rnd(0.2, 0.7), 0);
      t2geometry.rotateX(-PI / 2);
      rdnGeo(t2geometry, tradius * 0.1);
      tgeometry.merge(t2geometry);

      // body 2
      const b2size = bsize * rnd(0.5, 0.8);
      const t2bp = getTrunkBodyPosition(t2geometry, b2size);
      const b2geometry = new SphereGeometry(b2size, 4, 4);
      b2geometry.translate(t2bp.x, t2bp.y, t2bp.z);
      rdnGeo(b2geometry, b2size * 0.2);
      bgeometry.merge(b2geometry);
    }

    return tree;
  }

  // low poly rock
  function createRock(size) {
    const material = new MeshLambertMaterial({ color: 0x808080, flatShading: true });
    const geometry = new SphereGeometry(size, 5, 4);
    rdnGeo(geometry, size * 0.2);
    return new Mesh(geometry, material);
  }

  // trunk helper
  function getTrunkBodyPosition(geo, bsize) {
    let v1 = geo.vertices[0],v2 = geo.vertices[geo.vertices.length - 1];
    v1 = new Vector3(v1.x, v1.y, v1.z);
    v2 = new Vector3(v2.x, v2.y, v2.z);
    const dv = v1.clone().sub(v2).normalize().multiplyScalar(bsize * 0.5);
    return v1.add(dv);
  }

  // randomize geometry
  function rdnGeo(geo, d) {
    let v;
    for (let i = 0; i < geo.vertices.length; i++) {
      v = geo.vertices[i];
      v.x += rndFS(2 * d);
      v.y += rndFS(2 * d);
      v.z += rndFS(2 * d);
    }
    geo.computeFlatVertexNormals();
  }

  function animate() {
    requestAnimationFrame(animate);
    if (cameraCtrl) cameraCtrl.update();
    renderer.render(scene, camera);
  }

  function updateSize() {
    width = window.innerWidth;
    height = window.innerHeight;
    renderer.setSize(width, height);
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
  }
}

function getFibonacciSpherePoints(samples, radius, randomize) {
  samples = samples || 1;
  radius = radius || 1;
  randomize = randomize || true;
  let random = 1;
  if (randomize) {
    random = Math.random() * samples;
  }
  let points = [];
  let offset = 2 / samples;
  let increment = Math.PI * (3 - Math.sqrt(5));
  for (let i = 0; i < samples; i++) {
    let y = i * offset - 1 + offset / 2;
    let distance = Math.sqrt(1 - Math.pow(y, 2));
    let phi = (i + random) % samples * increment;
    let x = Math.cos(phi) * distance;
    let z = Math.sin(phi) * distance;
    x = x * radius;
    y = y * radius;
    z = z * radius;
    points.push({ x, y, z });
  }
  return points;
}
window['canvas_word'] = App;