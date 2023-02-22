using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trap Prediction script

// Shows the predicted path at which the trap object
// will follow as it flies through the air before landing

public class TrapPrediction : MonoBehaviour
{
    TrapShot trapShot;
    LineRenderer lineRenderer;

    public int numOfPoints = 50;

    public float timeBetweenPoints = 0.1f;

    public LayerMask CollidableLayers;


    // Start is called before the first frame update
    void Start()
    {
        // Set components to their specific variables

        trapShot = GetComponent<TrapShot>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Specifies how many points are rendered within
        // the line renderer

        lineRenderer.positionCount = numOfPoints;

        // Creates a new points list for determining position of points
        List<Vector3> points = new List<Vector3>();

        // gather starting position and velocity of object following
        // the line as to correctly predict the objects path

        Vector3 startingPosition = trapShot.shotPoint.position;
        Vector3 startingVelocity = trapShot.shotPoint.up * trapShot.shotForce;

        // loops through and creates each point at the correct location

        for (float i = 0; i < numOfPoints; i += timeBetweenPoints)
        {
            // sets a new point based on the objects velocity

            Vector3 newPoint = startingPosition + i * startingVelocity;

            // calculates the new points Y value based on an equation of motion
            // taking into account the starting position and velocity of the object and gravity
            // divided by the time between each point
            // then add this point to the list

            newPoint.y = startingPosition.y + startingVelocity.y * i + Physics.gravity.y / 2f * i * i;
            points.Add(newPoint);

            // Checks if the line is colliding with anything 
            // and if so stop drawing the points
            if (Physics.OverlapSphere(newPoint, 0.5f, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
        }


        // sets the position of each point based on the array
        lineRenderer.SetPositions(points.ToArray());
    }
}
