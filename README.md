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
- tag비교 시에 if other.gameObject.tag == "BULLET" 처럼 하면 GC때문에 퍼포먼스 저하(https://answers.unity.com/questions/200820/is-comparetag-better-than-gameobjecttag-performanc.html)
- Barrel에 Texture []로 3장의 텍스쳐를 할당한 후, 랜덤하게 1개의 텍스쳐를 갖도록
- 텍스쳐를 바로 그릴 수 없으니 MeshRenderer 에 전달해줘야 한다.
- Bullet Prefab에 TrailRenderer 추가. 다운받은 리소스에서 trail.png 가져와서 Material에 적용.
- 위 Material shader를 mobile/particle/additive 에 trail.png
- Muzzle Flash(Mesh에 그리기)
  - FirePos의 child로 Quad생성(이름:MuzzleFlash), 이 오브젝트에 MuzzleFlash.png를 끌어다 놓으면 자동으로 Materials안에 Material이 생성됨. 위와 마찬가지로 mobile/particle/additive하고 tiling 1/2로 설정
  - Coroutine 사용해서 깜빡거리게 구현
  - 좀 더 재밌어보이기 위해 발사할때마다 크기와 각도, texture offset을 Random하게 설정
    - 꼭 transform을 public으로 빼지 않아도 다른 컴포넌트.transform으로도 접근 가능(여기선 muzzleFlash라는 MeshRenderer로 접근) 
    - rotation을 하지 않고 setTextureOffset으로 텍스쳐를 갈아끼우는 방식으로도 Variation을 줄 수 있다.
- Unity 하늘 표현 방식
  - 1. Procedural Sky -> Sun, Atmosphere thickness 와 같은 Parameter를 수정하여 표현
  - 2. SkyBox(지금은 6 sided 라고 부름) -> 단점:6장의 텍스쳐 사용해야함.
  - 3. Sky Dome(가장자리 왜곡고려해야하지만 1장의 텍스쳐만 사용)
  - 4. Cubemap
  - 이 과정에선 1,2번 실습
  - Material 생성(SkyboxPC) -> Shader -> Procedural Sky선택, 파라미터 원하는대로조절. 
  - Window -> Lighting -> Environment에서 Material 교체, 이게 귀찮으면 그냥 Skybox Material을 scene에 drag해도 된다.
- Monster Model 다운로드(줌 채팅)
  - rig -> Humanoid 로 설정 -> Apply -> Configure 진입하여 Avatar멥핑정보 확인
  - Hierachy에 추가하여 이러한 모델을 가져왔을경우 Unit Scale, Animator인지 Animation인지 확인
  - 추가한 Monster는 Mechanim방식이기때문에 Animator컴포넌트를 갖고있고, 이는 script가 아닌 별도의 Controller를 필요로 함
  - Animator Controller(MonsterAnim)추가. Shift + Space 창 최대/최소화
    - FSM(Finite State Machine) 구조로 되어있음
    - Transition의 HasExitTime ? 헷갈리지만, 어떤 트리거가 발동되면 바로 트랜지션이 작동되길원하면 Uncheck되어야 함
    - RunTime에서 IsTrace(bool Parameter) 체크발동해제 해보면서 Transition잘 되는지 확인  

## 3주차 (2020.09.26, 온라인)
- mixamo에서 모션캡쳐 애니메이션 적용해보기
- Nav Mesh
  - 장애물 prefab에 Navigation Static 설정
  - Aera Mask ? Nav Mesh마다 cost를 다르게 줄 수 있다.
- Nav Mesh Agent
  - Monster -> Player 따라가도록 하는 스크립트(Moster Ctrl)
  - "Player"와 같이 오브젝트명으로도 reference할 수 있지만, tag로 하자(PLAYER 태그 추가)
  - FindGameObjectWithTag와 같은 find계열은 매우 느리기 때문에 initialize단계에서만 1번 실행되도록 주의
  - public 변수 Inspector에서 숨기기
  [System.NonSerialized],[HideInInspector]
  - script에서 Nav Mesh사용하려면 namespace추가해야함
    - using UnityEngine.AI;
    - monster의 상태체크는 FSM enum을 만든 후 관리하고, update에서 하지 않고 0.3f정도의 인터벌을 갖는 코루틴으로 체크해서 최적화한다.
  - Animator Hashtable에서 미리 Hash 추출하기
    - anim.set~(param, ..)에서 param에 문자열로 넘길 수도있고 해시값을 넘겨도 된다.
    - Hash값은 Integer(Animator.StringToHash(string))
  - gothit 애니메이션클립추가
    - 이 클립은 언제든지 작동할 수 있어야 하기 때문에 Any State에서 transition을 연결. 되돌아가는 transition은 모든상태로 각각 parameter 조건을 따져서 이어줘야한다.
    - Die 클립에서 RootTransformRotation, RootTransformPosition(Y)에서 Bake Into Pose 체크하면 바닥에 딱 붙게 됨
    - Die 조건에서는 모든 코루틴 중지
  - Monster Attack
    - wrist컴포넌트 찾아가서 sphere collider추가
    - IsTrigger체크해서 때려도 부딪히지 않도록, IsKinematic체크해서 BackGround에서 물리연산하지않도록(즉, 이 경우에는 UseGravity 무의미), RigidBody는 오로지 충돌체크를 위해 존재
    - OnTriggerEnter vs OnCollisionEnter
      - IsTrigger 체크/언체크, 관통/충돌
    - Layer 편집(몸통과 주먹 안부딪히게 하기위해)
      -  Sorting Layer가 포토샵에서같은 계층레이어라서 2D게임에서 사용. 그냥 Layer는 계층은 아니고 그룹의 개념이다.
      -  몸통 - MONSTER_BODY, 주먹 - MONSTER_PUNCH, Recursive this object only
      -  Edit -> Project Settings -> Physics -> Layer Collision Matrix에서 BODY, PUNCH 체크 해제
 - 사용하지않는 Gameobject Hierachy뷰에서 안보이게하기
   - 3D Model Inspector에서 -> Rig -> Extra Transforms to Expose에서 보이고싶은것만 체크
 - Monster 동적생성
   - SpawnPointGroup Empty object추가
   - UserDefinedGizmo를 스크립트로 만들어보기(MyGizmos.cs)
 - GameManager
   - spawnpoint, gameover등 게임관리하는 객체
   - 몬스터 생성 코루틴에서 yield return 시에 new 생성자 사용하지 않고, WaitForSeconds변수를 미리 start에서 생성하기(최적화)
  - SendMessage활용 PlayerDie()
    - SendMessage(FunctionName, receiveroption)
    - isGameOver는 public static으로 설정
    - 다만 여기서 Deligate를 활용하여 모든 Monster Object를 탐색하는것을 개선할 수 있음(OnEnable에서 += 연산자로 binding 시켜줌)
  - Object Pooling
    - Unity에서 Instantiate와 Destroy는 Cost가 크다.
    - 특히 Mobile에서는 필수로 구현해야함
  - GamaManager의 isGameOver의 Static키워드 삭제
    - PlayerCtrl에서 다른방법으로 접근
    - GameObject.Find("GameManager")으로 gamaManager변수를 할당해도 되지만, 이렇게 하면 모든 scipt변수를 선언해야하니 번거롭다. Singleton패턴으로 해결
  - Physics.Raycast
    - 총알 Collider를 사용하지 않고 Raycast 피격구현
    - MonsterCtrl에서 OnCollisionEnter대신 public OnDamage()와 같은 함수를 FireCtrl에서 호출
  - Nav Mesh Obstacle로 Nav Mesh Agent가 동적으로 장애물에 반응하게 할 수 있다.

## 4주차 (2020.10.10, 온라인)
