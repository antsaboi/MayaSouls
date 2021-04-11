using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PlatformerMovementWASD))]
public class PlatformerMovementWASDEditor : Editor
{
    PlatformerMovementWASD _base;

    private void OnEnable()
    {
        _base = (PlatformerMovementWASD)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //keys
        SerializedProperty axis = serializedObject.FindProperty("UseAxis");
        SerializedProperty up = serializedObject.FindProperty("up");
        SerializedProperty down = serializedObject.FindProperty("down");
        SerializedProperty left = serializedObject.FindProperty("left");
        SerializedProperty right = serializedObject.FindProperty("right");
        SerializedProperty jump = serializedObject.FindProperty("jump");

        //Movement
        SerializedProperty speed = serializedObject.FindProperty("moveSpeed");
        SerializedProperty minMoveSpeed = serializedObject.FindProperty("minMoveSpeed");
        SerializedProperty airSpeed = serializedObject.FindProperty("airMoveSpeed");
        SerializedProperty maxSpeed = serializedObject.FindProperty("maxSpeed");
        SerializedProperty jumpForce = serializedObject.FindProperty("jumpForce");
        SerializedProperty jumpHeight = serializedObject.FindProperty("jumpHeight");
        SerializedProperty timeToJumpApex = serializedObject.FindProperty("timeToJumpApex");
        SerializedProperty gravityScale = serializedObject.FindProperty("gravityScale");
        SerializedProperty constraints = serializedObject.FindProperty("constraints");
        SerializedProperty speedLimitMode = serializedObject.FindProperty("speedLimitMode"); //if rbd
        SerializedProperty movementLockAxis = serializedObject.FindProperty("movementLockAxis"); //not sure if applicable tho
        SerializedProperty moveMethod = serializedObject.FindProperty("moveMethod"); // make tabs
        SerializedProperty horizontalRayCount = serializedObject.FindProperty("horizontalRayCount");
        SerializedProperty verticalRayCount = serializedObject.FindProperty("verticalRayCount");
        SerializedProperty collisionMask = serializedObject.FindProperty("collisionMask");
        SerializedProperty accelerationTimeAirborne = serializedObject.FindProperty("accelerationTimeAirborne");
        SerializedProperty accelerationTimeGrounded = serializedObject.FindProperty("accelerationTimeGrounded");
        SerializedProperty decelerationTimeGrounded = serializedObject.FindProperty("decelerationTimeGrounded");
        SerializedProperty alignToSurface = serializedObject.FindProperty("alignToSurface");
        SerializedProperty alignToSurfaceSmoothing = serializedObject.FindProperty("alignToSurfaceSmoothing");
        
        //Events
        SerializedProperty OnMoveEvent = serializedObject.FindProperty("OnMoveEvent");
        SerializedProperty OnStartMovingEvent = serializedObject.FindProperty("OnStartMovingEvent");
        SerializedProperty OnStopEvent = serializedObject.FindProperty("OnStopEvent");

        //Drawing properties

        //Change guistyle based on active state
        moveMethod.enumValueIndex = (int)GUILayout.Toolbar((int)moveMethod.enumValueIndex, System.Enum.GetNames(typeof(PlatformerMovementWASD.MoveMethod)));

        GUILayout.Space(10f);

        EditorGUILayout.LabelField("Keys", new GUIStyle("boldLabel"));

        string buttonText = axis.boolValue ? "Using Axis - Press to Select Keys" : "Use Axis instead of Keys";

        if (GUILayout.Button(buttonText))
        {
            axis.boolValue = !axis.boolValue;
        }

        GUILayout.Space(10f);
        DrawMovementKeys(up, down, left, right, jump, axis.boolValue);

        GUILayout.Space(20f);

        EditorGUILayout.LabelField("Move Settings", new GUIStyle("boldLabel"));
        EditorGUILayout.PropertyField(alignToSurface);
        EditorGUILayout.PropertyField(alignToSurfaceSmoothing);
        
        EditorGUILayout.PropertyField(collisionMask);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(minMoveSpeed);
        EditorGUILayout.PropertyField(airSpeed);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(maxSpeed);
        EditorGUILayout.PropertyField(speedLimitMode, GUIContent.none, GUILayout.MaxWidth(120f));
        EditorGUILayout.EndHorizontal();
        

        //EditorGUILayout.PropertyField(movementLockAxis);



        switch ((PlatformerMovementWASD.MoveMethod)moveMethod.enumValueIndex)
        {
            case PlatformerMovementWASD.MoveMethod.RigidbodyAddForce:
                EditorGUILayout.PropertyField(gravityScale);
                if (gravityScale.floatValue <= 0)
                {
                    EditorGUILayout.HelpBox("Gravity scale needs to be positive", MessageType.Info);
                    gravityScale.floatValue = 0;
                }
                EditorGUILayout.PropertyField(constraints);
                EditorGUILayout.PropertyField(jumpForce);

                break;
            case PlatformerMovementWASD.MoveMethod.RigidbodyVelocity:
                EditorGUILayout.PropertyField(gravityScale);
                if (gravityScale.floatValue <= 0)
                {
                    EditorGUILayout.HelpBox("Gravity scale needs to be positive", MessageType.Info);
                    gravityScale.floatValue = 0;
                }

                EditorGUILayout.PropertyField(constraints);
                EditorGUILayout.PropertyField(jumpForce);

                break;
            case PlatformerMovementWASD.MoveMethod.Raycast:

                /*                if (gravityScale.floatValue >= 0)
                                {
                                    EditorGUILayout.HelpBox("Gravity scale needs to be negative", MessageType.Info);
                                    gravityScale.floatValue = 0;
                                }*/
                EditorGUILayout.PropertyField(accelerationTimeAirborne);
                EditorGUILayout.PropertyField(accelerationTimeGrounded);
                EditorGUILayout.PropertyField(decelerationTimeGrounded);
                EditorGUILayout.PropertyField(horizontalRayCount);
                EditorGUILayout.PropertyField(verticalRayCount);
                EditorGUILayout.PropertyField(jumpHeight);
                EditorGUILayout.PropertyField(timeToJumpApex);

                break;
            default:
                break;
        }

        GUILayout.Space(20f);
        EditorGUILayout.PropertyField(OnStartMovingEvent);
        EditorGUILayout.PropertyField(OnStopEvent);
        EditorGUILayout.PropertyField(OnMoveEvent);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMovementKeys(SerializedProperty up, SerializedProperty down, SerializedProperty left, SerializedProperty right, SerializedProperty jump, bool useAxis)
    {
        if (useAxis)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            jump.enumValueIndex = EditorGUILayout.Popup(jump.enumValueIndex, jump.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        up.enumValueIndex = EditorGUILayout.Popup(up.enumValueIndex, up.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        left.enumValueIndex = EditorGUILayout.Popup(left.enumValueIndex, left.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(30));
        down.enumValueIndex = EditorGUILayout.Popup(down.enumValueIndex, down.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(30));
        right.enumValueIndex = EditorGUILayout.Popup(right.enumValueIndex, right.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(30));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        jump.enumValueIndex = EditorGUILayout.Popup(jump.enumValueIndex, jump.enumDisplayNames, new GUIStyle("button"), GUILayout.Height(30), GUILayout.Width(100));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}

#endif

[CreateAssetMenu(fileName = "MovementBehaviour2D", menuName = "Scriptables/ProtoPlayer2D/Movement")]
public class PlatformerMovementWASD : ProtoPlayerBehaviourBase
{
    public bool UseAxis;
    public KeyCode up = KeyCode.W, down = KeyCode.S, left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space;

    public bool alignToSurface;

    public float moveSpeed, minMoveSpeed, airMoveSpeed;
    public Vector2 maxSpeed;
    public float jumpHeight = 4, timeToJumpApex = .4f;
    public float wallSlideSpeedMax = 3; //Make public?
    public float jumpForce = 10;
    public float gravityScale = 1;
    public bool grounded;

    public RigidbodyConstraints2D constraints;
    public enum SpeedLimitMode
    {
        None,
        Strict
    }
    [Tooltip("Strict = speed can never go above abs value, None = no limit")]
    public SpeedLimitMode speedLimitMode; //if rbd

    public enum LockAxis
    {
        None,
        Vertical,
        Horizontal
    }

    public LockAxis movementLockAxis;
    public enum MoveMethod {
        RigidbodyAddForce,
        RigidbodyVelocity,
        Raycast
    }
    public MoveMethod moveMethod;

    [Header("Event Hooks")]
    public UnityEvent OnMoveEvent, OnStartMovingEvent, OnStopEvent;
    private Action _onKeys;
    private const string _horizontal = "Horizontal", _vertical = "Vertical";

    //Need to reset this runtimedata
    private Rigidbody2D _body;
    [HideInInspector] public Vector2 currentVelocity;
    private bool _isGrounded;
    private bool _movingStart;

    private float _maxClimbAngle = 80;
    private float _maxDescendAngle = 75;
    public float alignToSurfaceSmoothing = 5f;

    //Raycast Move methods

    private CollisionInfo _collisions;
    private float velocityXSmoothing;
    //Mess with these to handle acceleration
    public float accelerationTimeAirborne = .2f, accelerationTimeGrounded = .1f, decelerationTimeGrounded = 0;
    public bool hanging = false;
    private Vector2 knockBack;

    #region raycast controller things
    //Raycast controller
    private float _horizontalRaySpacing, _verticalRaySpacing;
    RaycastOrigins raycastOrigins;
    private Collider2D _collider;
    public int horizontalRayCount = 4, verticalRayCount = 4;
    public LayerMask collisionMask;
    const float skinWidth = 0.015f;
    float jumpTime = 0.05f;
    float jumpTimeStamp;

    #endregion

    public override void StartBehaviour()
    {
        ResetRuntimeVars();

        _collider = playerInstance.GetComponent<BoxCollider2D>();

        if (moveMethod == MoveMethod.RigidbodyAddForce || moveMethod == MoveMethod.RigidbodyVelocity)
        {
            if (playerInstance.GetComponent<Rigidbody2D>() != null) _body = playerInstance.GetComponent<Rigidbody2D>();
            else _body = playerInstance.AddComponent<Rigidbody2D>();
            _body.constraints = constraints;
            _body.gravityScale = gravityScale;
            _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            if (_collider != null)
            {
                Destroy(_collider);
                playerInstance.AddComponent<CapsuleCollider2D>();
            }
        }
        else {
/*            if (playerInstance.GetComponent<Rigidbody2D>() != null) Destroy(playerInstance.GetComponent<Rigidbody2D>());*/
            Physics2D.autoSyncTransforms = true;
        }

        switch (moveMethod)
        {
            case MoveMethod.RigidbodyAddForce:
                _onKeys += MoveWithAddForce;
                break;
            case MoveMethod.RigidbodyVelocity:
                _onKeys += MoveWithVelocity;
                break;
            case MoveMethod.Raycast:
                CalculateRaySpacing();
                gravityScale = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                jumpForce = Mathf.Abs(gravityScale) * timeToJumpApex;
                _collisions.faceDirection = 1;
                _onKeys += MoveWithRaycast;
                break;
            default:
                break;
        }

        _onKeys += Jump;
    }

    public void StartedMoving()
    {
        Debug.Log("Started Moving!");
    }

    public override void UpdateBehaviour()
    {
        _onKeys?.Invoke();
    }

    #region Rigidbody Moving Methods
    private void MoveWithAddForce()
    {
        currentVelocity = _body.velocity;
        if (UseAxis)
        {

        }
        else
        {
            //Add keycode implementation here    
        }
    }

    private void MoveWithVelocity()
    {
        float currentDir = Mathf.Sign(currentVelocity.x);
        currentVelocity = _body.velocity;
        GroundCheck();

        if (playerInstance.transform.localScale.x != currentDir && _isGrounded)
        {
            playerInstance.transform.localScale = new Vector2(currentDir, playerInstance.transform.localScale.y);
        }

        if (alignToSurface)
        {
            if (_isGrounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(playerInstance.transform.position, Vector2.down, 2f, collisionMask);

                if (hit)
                {
                    Quaternion targetAngle = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    playerInstance.transform.rotation = Quaternion.Slerp(playerInstance.transform.rotation, targetAngle, alignToSurfaceSmoothing *Time.deltaTime);
                }
            }
            else {
                playerInstance.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
        }

        if (UseAxis)
        {
            float hAxis = Input.GetAxis(_horizontal);
            float vAxis = Input.GetAxis(_vertical);

            if (!_movingStart && Mathf.Abs(hAxis) > 0)
            {
                OnStartMovingEvent?.Invoke();
                _movingStart = true;
            }

            if (hAxis == 0)
            {
                _movingStart = false;
            }

            if (_isGrounded)
            {
                if (currentVelocity.x < maxSpeed.x && Mathf.Abs(hAxis) > 0)
                {
                    _body.velocity = new Vector2(moveSpeed * hAxis, _body.velocity.y);
                }
            }
            else {
                //In air
                if (currentVelocity.x < maxSpeed.x && Mathf.Abs(hAxis) > 0)
                {
                    _body.velocity = new Vector2(moveSpeed * hAxis, _body.velocity.y);
                }
            }

            if (speedLimitMode == SpeedLimitMode.Strict)
            {
                if (Mathf.Abs(_body.velocity.x) >= maxSpeed.x)
                {
                    if (_body.velocity.x > 0)
                    {
                        _body.velocity = new Vector2(maxSpeed.x, _body.velocity.y);
                    }
                    else
                    {
                        _body.velocity = new Vector2(-maxSpeed.x, _body.velocity.y);
                    }
                }

                if (Mathf.Abs(_body.velocity.y) >= maxSpeed.y)
                {
                    if (_body.velocity.y > 0)
                    {
                        _body.velocity = new Vector2(_body.velocity.x, maxSpeed.y);
                    }
                    else
                    {
                        _body.velocity = new Vector2(_body.velocity.x, -maxSpeed.y);
                    }
                }
            }
        }
        else {
            //Add keycode implementation here    
        }
    }

    bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerInstance.transform.position, -playerInstance.transform.up, 1f, collisionMask);
        Debug.DrawRay(playerInstance.transform.position, -playerInstance.transform.up, Color.red, 1f);
        if (hit)
        {
            _isGrounded = true;
        }
        else _isGrounded = false;
        return _isGrounded;
    }

    #endregion

    #region Raycast Moving Method

    public void TakeDamage(Vector2 knockBack)
    {
        this.knockBack = knockBack;
    }

    private void MoveWithRaycast()
    {
        if (UseAxis)
        {
            if (_collisions.above || _collisions.below)
            {
                currentVelocity.y = 0;
            }

            Vector2 input = new Vector2(Input.GetAxisRaw(_horizontal), Input.GetAxisRaw(_vertical));

            if (!GameManager.instance.isAlive) input = Vector2.zero;

            if ((Input.GetKeyDown(jump) || Input.GetKeyDown(KeyCode.Space)) && grounded && Time.time > jumpTimeStamp)
            {
                if (GameManager.instance.isAlive) currentVelocity.y = jumpForce;
            }

            float targetVeloX = input.x * moveSpeed;

            float currentDir = Mathf.Sign(currentVelocity.x);
            float targetDir = Mathf.Sign(targetVeloX);

            if (knockBack != Vector2.zero)
            {
                currentVelocity = new Vector2(-knockBack.x * currentDir, knockBack.y);
                knockBack = Vector2.zero;
            }

            if (Mathf.Abs(targetVeloX) > 0 && playerInstance.transform.localScale.x != targetDir)
            {
                FlipRaycastOrigins();
                playerInstance.transform.localScale = new Vector2(targetDir, playerInstance.transform.localScale.y);
            }

            if (input.x == 0)
            {
                //Stopping, apply decelerationTime on ground
                currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, targetVeloX, ref velocityXSmoothing, _collisions.below ? decelerationTimeGrounded : accelerationTimeAirborne);
            }
            else
            {

                if (Mathf.Abs(currentVelocity.x) < minMoveSpeed && Mathf.Abs(input.x) > 0 && _collisions.below)
                {
                    //apply min speed
                    currentVelocity.x = targetDir * minMoveSpeed;
                }
                else if (currentDir != targetDir && _collisions.below)
                {
                    //Quick change in direction, apply min speed
                    //apply min speed
                    currentVelocity.x = targetDir * minMoveSpeed;
                }
                else
                {
                    currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, targetVeloX, ref velocityXSmoothing, _collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
                }
            }
            currentVelocity.y += gravityScale * Time.deltaTime;
            MovePlayer(currentVelocity * Time.deltaTime);
        }
        if (_collisions.below)
        {
            if (!grounded)
            {
                jumpTimeStamp = Time.time + jumpTime;
            }

            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    //Used for Raycast version!
    public void MovePlayer(Vector2 velocity, bool standingOnPlatform = false)
    {
        //UpdateRaycastOrigins();

        _collisions.Reset();
        _collisions.velocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if (velocity.x != 0)
        {
            _collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }

        HorizontalCollisions(ref velocity);
        
        if(velocity.y != 0)
        VerticalCollisions(ref velocity);
        //add stuff here

        playerInstance.transform.Translate(velocity);

        if (standingOnPlatform) _collisions.below = true;

        if (alignToSurface)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerInstance.transform.position, Vector2.down, 1f, collisionMask);

            if (hit)
            {
                Quaternion targetAngle = Quaternion.FromToRotation(Vector3.up, hit.normal);
                playerInstance.transform.rotation = Quaternion.Slerp(playerInstance.transform.rotation, targetAngle, alignToSurfaceSmoothing * Time.deltaTime);
            }
            else
            {
                playerInstance.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
        }
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float dirX = _collisions.faceDirection;
        float dirY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Transform rayOrigin = dirX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            Vector2 rayPos = (Vector2)rayOrigin.position + (Vector2)(rayOrigin.transform.up * (_horizontalRaySpacing * i));
            RaycastHit2D hit = Physics2D.Raycast(rayPos, rayOrigin.transform.right * dirX, rayLength, collisionMask);

            Debug.DrawRay(rayPos, rayOrigin.transform.right * dirX * rayLength, Color.red);

            if (hit)
            {
                if (hit.distance == 0) continue;

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= _maxClimbAngle)
                {
                    if (_collisions.descendingSlope)
                    {
                        _collisions.descendingSlope = false;
                        velocity = _collisions.velocityOld;
                    }

                    float distanceToSlopeStart = 0;
                    if (slopeAngle != _collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * dirX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * dirX;
                }

                if (!_collisions.climbingSlope || slopeAngle > _maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * dirX;
                    rayLength = hit.distance;

                    if (_collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    _collisions.left = dirX == -1;
                    _collisions.right = dirX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float dirY = Mathf.Sign(velocity.y);
        float dirX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Transform rayOrigin = dirY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            
            Vector2 rayPos = (Vector2)rayOrigin.transform.position + (Vector2)(rayOrigin.transform.right * (_verticalRaySpacing * i + velocity.x));
            RaycastHit2D hit = Physics2D.Raycast(rayPos, rayOrigin.transform.up * dirY, rayLength, collisionMask);

            Debug.DrawRay(rayPos, rayOrigin.transform.up * dirY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * dirY;
                rayLength = hit.distance;

                if (_collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                _collisions.below = dirY == -1;
                _collisions.above = dirY == 1;
            }
        }

        if (_collisions.climbingSlope)
        {
            rayLength = Mathf.Abs(velocity.x) + skinWidth + 0.1f;
            Transform rayOrigin = ((dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, dirX == -1 ? -rayOrigin.transform.right : rayOrigin.transform.right, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != _collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * dirX;
                    _collisions.slopeAngle = slopeAngle;
                }

            }
        }
    }

    void ClimbSlope(ref Vector2 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY) {

            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            _collisions.below = true;
            _collisions.climbingSlope = true;
            _collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 velocity)
    {
        float dirX = Mathf.Sign(velocity.x);

        Transform rayOrigin = (dirX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, -rayOrigin.transform.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle != 0 && slopeAngle <= _maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == dirX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        _collisions.slopeAngle = slopeAngle;
                        _collisions.descendingSlope = true;
                        _collisions.below = true;
                    }
                }
            }
        }
    }

    public struct CollisionInfo
    {
        public int faceDirection;
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;
        public bool descendingSlope;
        public bool ropeHanging;
        public Vector3 velocityOld;

        public void Reset()
        {
            climbingSlope = false;
            descendingSlope = false;
            above = false;
            below = false;
            left = false;
            right = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    struct RaycastOrigins
    {
        public Transform topLeft, topRight;
        public Transform bottomLeft, bottomRight;
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);
        Debug.Log("Calculating bounds");

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

        raycastOrigins.bottomLeft = new GameObject("bottomLeft").transform;
        raycastOrigins.bottomLeft.transform.parent = playerInstance.transform;
        raycastOrigins.bottomLeft.transform.position = new Vector2(bounds.min.x, bounds.min.y);

        raycastOrigins.bottomRight = new GameObject("bottomRight").transform;
        raycastOrigins.bottomRight.transform.parent = playerInstance.transform;
        raycastOrigins.bottomRight.transform.position = new Vector2(bounds.max.x, bounds.min.y);

        raycastOrigins.topLeft = new GameObject("topLeft").transform;
        raycastOrigins.topLeft.transform.parent = playerInstance.transform;
        raycastOrigins.topLeft.transform.position = new Vector2(bounds.min.x, bounds.max.y);

        raycastOrigins.topRight = new GameObject("topRight").transform;
        raycastOrigins.topRight.transform.parent = playerInstance.transform;
        raycastOrigins.topRight.transform.position = new Vector2(bounds.max.x, bounds.max.y);
    }

    void FlipRaycastOrigins()
    {
            Transform swapper = null;
            swapper = raycastOrigins.bottomLeft;
            raycastOrigins.bottomLeft = raycastOrigins.bottomRight;
            raycastOrigins.bottomRight = swapper;

            swapper = raycastOrigins.topLeft;
            raycastOrigins.topLeft = raycastOrigins.topRight;
            raycastOrigins.topRight = swapper;
    }

/*    void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }*/

    private void ResetRuntimeVars()
    {
        _body = null;
        _collider = null;
        currentVelocity = Vector2.zero;
        _isGrounded = false;
        _movingStart = false;
        _horizontalRaySpacing = 0;
        _verticalRaySpacing = 0;
    }


    #endregion

    #region Jump and OnDestroy
    private void Jump()
    {
        switch (moveMethod)
        {
            case MoveMethod.RigidbodyAddForce:
                if ((Input.GetKeyDown(jump) || Input.GetKeyDown(KeyCode.Space)) && _isGrounded)
                {
                    _body.velocity = new Vector2(currentVelocity.x, jumpForce);
                }
                break;
            case MoveMethod.RigidbodyVelocity:
                if ((Input.GetKeyDown(jump) || Input.GetKeyDown(KeyCode.Space)) && _isGrounded)
                {
                    _body.velocity = new Vector2(currentVelocity.x, jumpForce);
                }
                break;
            case MoveMethod.Raycast:


                break;
            default:
                break;
        }
    }

    public override void OnDestroyBehaviour()
    {
        switch (moveMethod)
        {
            case MoveMethod.RigidbodyAddForce:
                _onKeys -= MoveWithAddForce;
                break;
            case MoveMethod.RigidbodyVelocity:
                _onKeys -= MoveWithVelocity;
                break;
            case MoveMethod.Raycast:
               _onKeys -= MoveWithRaycast;
                break;
            default:
                break;
        }

        _onKeys -= Jump;
    }

    #endregion
}