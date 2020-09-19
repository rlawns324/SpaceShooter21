# 성남 Connect21 주말 과정 
## 1주차 (2020.09.12)
- Unity 소개
- Unity hub 사용법, Unity 2020.1.5f 설치, VSCode 설치(Extension : C#, Unity Snippets Modified)
- Interface : Scene, Game, Hierachy, Project, Inspector..., Scene 조작 단축키(ctrl+alt+좌클릭(panning), alt+좌클릭(rotate), space(maximize), v(vertex 자석), ctrl+p(play/stop)...)
- Component Based Architecture : 모든 GameObject는 Empty Object로 부터 Mesh Filter, Mesh Renderer, Collider, Script 등 다양한 Component를 조립하여 만들 수 있다. Script의 Execution Order 설정 가능
- Material : Mesh에 Texture를 어떻게 입힐지에 대한 정보를 갖고있는 것, Albedo, NormalMap, Emission Texture 적용해보기
- Prefab : 복사본 생성 가능, Project에서 원본 Prefab 수정 시 모든 복사객체에 적용, 복사객체 하나만 수정하고 Apply 시킬 수도 있음, 이 경우에는 수정사항에 대해 Inspector에 뭔가 표시됨
- Script : Awake, OnEnable, Start, Update, FixedUpdate, LateUpdate 등 개념
- transform.position 하드코딩보단 가독성 및 최적화를 위해, Unity 내장함수 사용 권장(ex: transform.Translate, transform.Rotate, Time.deltaTime으로 디바이스별 프레임보정, Vector3.backward 이런거도 있긴 하지만, 왼손좌표계 기준으로 생각하고 Vector3.forward * -1 을 권장 
- Unity Animation
  - Legacy(Script방식, 약간 더 빠르다.) : Component이름 "Animation"
  - Mechanim(Node 기반의 Visual Scripting) : Component이름 "Animator"
    - Generic
    - Humanoid(2족보행, 15개의 필수 Bone) -> Retargeting 
  - 간단한 Animation은 Legacy방식이 더 편할때가 많다.
  - Animation 컴포넌트의 Animation 속성은, Default Animation Clip이다. 즉 반드시 Animations 속성안에 있는 클립이어야 한다.

- UserDefine Class를 만들어서 public변수 생성했을 경우, Script에서 "[System.Serializable]" 해줘야 한다.(C# <-> C++ 직렬화)
- Animation.CrossFade : Animation Clip간에 smooth하게 보간
- LOD(Level Of Detail) : 예제 Player모델이 이상했었는데 LOD가 짬뽕돼있었기 때문. LOD Group Component 제공
- Skinned Mesh Renderer : Mesh filter + Mesh Renderer 둘다 가지고 있는 Component인데, Animation을 갖고 있는 3D Model을 Import할 경우 Unity 에서 이 Component로 치환한다.
- Rigidbody : 물리엔진과 작용하게하는 Component
  - IsKinematic ? 운동역학으로 움직이냐?하는 bool, 즉 script에서 traform을 제어하는지 유무. 움직이는 script가 없고 IsKinematic이 체크 돼있으면 움직이지 않는다. script로 움직이는 물체에 특정시점에 물리엔진을 먹이고 싶을때 사용(ex: 폭발), 강좌 후반에 ragdoll할때 실습할 예정 
  - Interpolate/Extrapolate ? Update/FixedUpdate간의 차이(Gittering 현상 발생) 를 보간하는데 전 프레임으로 할거냐 다음프레임으로 할거냐 하는 옵션. 주로 Interpolate를 씀.
  - Collision Detection ? Discrete에서 밑에 Continuous 계열 옵션으로 갈수록 정밀도,부하 Up
  - Collider 연산 빠른순서 : 1.Sphere(r) 2.Capsule(r,h) 3.Box
    - Mesh Collider 는 주의해서 사용해야한다.(Low Poly에서는 잘 사용하면 매우 편리할 수 있다.)
- AddForce/AddRelativeForce : Global/Local 좌표 차이

## 2주차 (2020.09.19, 온라인)
- Weapon모델에 firePos위치 및 fireCtrl 스크립트 연동
- 적당히 기능별로 script를 분리하는것이 좋다.
- 동적으로 Object를 생성하는 메서드는 Instantiate가 유일하다.
- Bullet prefab에 Add Tag로 BULLET 추가(충돌 처리를 위해)
- OnCollisionEnter(Collision) 충돌처리 Callback
- Cinemachine(Package Manager -> Unity Registry -> Cinemachine Install)
  - Create Virtual Camera
  - Follow, LookAt 에 Player 할당 
  - Save During Play 체크 하면 Runtime에서 설정한 값들이 저장된다.
- Particle Effect : Asset Store -> Unity Particle Pack 5.x Download
- 총알에 충돌당한 면의 Normal방향으로 이펙트발생시키기
  - 충돌체 면이 파고들 수 있으므로, 충돌된 point는 1점 이상이어서 배열로 반환(Collision.contacts)된다. 
  - Quaternion.LookRotation(vec3) -> vec3의 각을 쿼터니언으로 변환해줌
- AudioListner -> 귀(유일하게 1개만 존재해야한다. Main Camera에 달려있는것 말고 다른 카메라에있는것은 삭제)
- AudioSource -> 음원(Player에 추가, FireCtrl 스크립트에서 제어)
  - PlayOneShot(AudioClip) : 중간에 안짜르고 1번 재생하는 메서드
- Asset Store -> Barrel Import -> Prefab말고 원본 모델에서 Scale Factor 줄이고 씬에 추가. Transform Scale을 줄이는것은 권장X
- Barrel 에 Capsule Collider 추가, RemoveBullet 스크립트 추가
- Barrel Texture Random 하게 되도록