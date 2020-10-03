using System;
using UnityEngine;
using System.Collections.Generic;
public static class Collision2DExtensions {
    public static Collision2DSideType GetContactSide (Vector2 max, Vector2 center, Vector2 contact) {
        Collision2DSideType side = Collision2DSideType.None;
        float diagonalAngle = Mathf.Atan2(max.y - center.y, max.x - center.x) * 180 / Mathf.PI;
        float contactAngle = Mathf.Atan2(contact.y - center.y, contact.x - center.x) * 180 / Mathf.PI;
        
        if (contactAngle < 0) {
            contactAngle = 360 + contactAngle;
        }

        if (diagonalAngle < 0) {
            diagonalAngle = 360 + diagonalAngle;
        }

        if (
            ((contactAngle >= 360 - diagonalAngle) && (contactAngle <= 360)) ||
            ((contactAngle <= diagonalAngle) && (contactAngle >= 0))
        ) {
            side = Collision2DSideType.Right;
        }
        else if (
            ((contactAngle >= 180 - diagonalAngle) && (contactAngle <= 180)) ||
            ((contactAngle >= 180) && (contactAngle <= 180 + diagonalAngle))
        ) {
            side = Collision2DSideType.Left;
        }
        else if (
            ((contactAngle >= diagonalAngle) && (contactAngle <= 90)) ||
            ((contactAngle >= 90) && (contactAngle <= 180 - diagonalAngle))
        ) {
            side = Collision2DSideType.Top;
        }
        else if (
            ((contactAngle >= 180 + diagonalAngle) && (contactAngle <= 270)) ||
            ((contactAngle >= 270) && (contactAngle <= 360 - diagonalAngle))
        ) {
            side = Collision2DSideType.Bottom;
        }

        return side.Opposite();
    }

    public static Collision2DSideType GetContactSide(this Collision2D collision, int i = 0) {
        Vector2 max = collision.otherCollider.bounds.max;
        Vector2 center = collision.otherCollider.bounds.center;
        Vector2 contact = collision.GetContact(i).point;
        return GetContactSide(max, center, contact);
    }
    public static int GetContactCount(this Collision2D collision)
    {
        return (collision.contactCount);
    }
}