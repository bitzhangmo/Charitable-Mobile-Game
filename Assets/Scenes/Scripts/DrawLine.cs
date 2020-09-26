using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    [Range(1,10)]
    public float _maxDistance = 10f;
    [Range(1,5)]
    public int _maxIterations = 3;

    public int _count;
    public LineRenderer _line;
    public Transform FirePoint;
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _line.SetVertexCount(1);
        _line.SetPosition(0,transform.position);
        _line.enabled = true;
        RayCast(transform.position, transform.up);
    }

    public void DrawThisLine(Vector2 dir)
    {
        _line.SetVertexCount(1);
        _line.SetPosition(0,transform.position);
        _line.enabled = true;
        RayCast(transform.position, dir);
    }
    private bool RayCast(Vector2 position, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, dir , _maxDistance);
        if (hit && _count <= _maxIterations - 1)
        {
            _count++;
            var reflectAngle = Vector2.Reflect(dir,hit.normal);
            _line.SetVertexCount(_count + 1);
            _line.SetPosition(_count, hit.point);
            RayCast(hit.point + reflectAngle, reflectAngle);
            return true;
        }

        if(hit == false)
        {
            _line.SetVertexCount(_count + 2);
            _line.SetPosition(_count + 1, position + dir*_maxDistance);
        }
        return false;
    }
}
