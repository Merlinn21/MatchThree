using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector3 firstPos;
    private Vector3 lastPos;
    private float swipeAngle;

    public float xPosition;
    public float yPosition;
    public int column;
    public int row;
    private Grid grid;
    private GameObject otherTile;

    public bool isMatched = false;

    private int previousColumn;
    private int previousRow;
    private GameManager gm;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        gm = FindObjectOfType<GameManager>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        column = Mathf.RoundToInt((xPosition - grid.startPos.x) / grid.offset.x);
        row = Mathf.RoundToInt((yPosition - grid.startPos.y) / grid.offset.x);
    }

    private void Update()
    {
        CheckMatches();
        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.grey;
        }

        xPosition = (column * grid.offset.x) + grid.startPos.x;
        yPosition = (row * grid.offset.y) + grid.startPos.y;
        if(!gm.gameOver)
            SwipeTile();
       
    }

    private void OnMouseDown()
    {
        firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(lastPos.y - firstPos.y, lastPos.x - firstPos.x) * 180 / Mathf.PI;
        MoveTile();
    }

    private void MoveTile()
    {
        previousColumn = column;
        previousRow = row;

        if (swipeAngle > -45 && swipeAngle <= 45)
            SwipeRightMove();
        else if (swipeAngle > 45 && swipeAngle <= 135)
            SwipeUpMove();
        else if (swipeAngle > 135 || swipeAngle <= -135)
            SwipeLeftMove();
        else if (swipeAngle < -45 && swipeAngle >= -135)
            SwipeDownMove();

        StartCoroutine(checkMove());
    }

    void SwipeRightMove()
    {
        if (column + 1 < grid.gridSizeX)
        {
            otherTile = grid.tiles[column + 1, row];
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
    }

    void SwipeUpMove()
    {
        if (row + 1 < grid.gridSizeY)
        {
            otherTile = grid.tiles[column, row + 1];
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
    }

    void SwipeLeftMove()
    {
        if (column - 1 >= 0)
        {
            otherTile = grid.tiles[column - 1, row];
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
    }

    void SwipeDownMove()
    {
        if (row - 1 >= 0)
        {
            otherTile = grid.tiles[column, row - 1];
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }
    }

    void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > .1)
        {
            Vector3 tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            Vector3 tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }

        if (Mathf.Abs(yPosition - transform.position.y) > .1)
        {
            Vector3 tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            Vector3 tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }
    }

    private void CheckMatches()
    {
        if(column > 0 && column < grid.gridSizeX - 1)
        {
            GameObject leftTile = grid.tiles[column - 1, row];
            GameObject rightTile = grid.tiles[column + 1, row];

            if(leftTile != null && rightTile != null)
            {
                if (leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    rightTile.GetComponent<Tile>().isMatched = true;
                    leftTile.GetComponent<Tile>().isMatched = true;
                }
            }

            if(row > 0 && row < grid.gridSizeY - 1)
            {
                GameObject upTile = grid.tiles[column, row + 1];
                GameObject downTile = grid.tiles[column, row - 1];

                if (upTile != null && downTile != null)
                {
                    if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                    {
                        isMatched = true;
                        downTile.GetComponent<Tile>().isMatched = true;
                        upTile.GetComponent<Tile>().isMatched = true;
                    }
                }
            }
        }
    }

    IEnumerator checkMove()
    {
        yield return new WaitForSeconds(.5f);
        if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;
                gm.multiplier = 1;
            }
            else
            {
                gm.multiplier += 0.25f;
                grid.DestroyMatches();
            }
        }
        otherTile = null;
    }


}
