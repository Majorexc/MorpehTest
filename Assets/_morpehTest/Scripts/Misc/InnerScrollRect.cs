 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class InnerScrollRect : ScrollRect
 {
     //Reference to outer scroll rect
     ScrollRect outerScroll;
 
     //Helper flags
     bool sendToOuter;
     bool enteringDrag;
 
     protected override void Awake ()
     {
         base.Awake ();
 
         //Try to find outer scrollrect
         outerScroll = this.transform.parent.GetComponentInParent<ScrollRect> ();
     }
 
     public override void OnInitializePotentialDrag (PointerEventData eventData)
     {
         //Send through potential drag event (stoping inertia movement on both
         //scrollrect)
         if (outerScroll)
             outerScroll.OnInitializePotentialDrag (eventData);
 
         base.OnInitializePotentialDrag (eventData);
     }
 
     public override void OnBeginDrag (PointerEventData eventData)
     {
         //React only to touch (button) to which scrollrect normaly react
         if (eventData.button != PointerEventData.InputButton.Left)
             return;
 
         //Set flag to know when first drag frame occurs
         enteringDrag = true;
 
         if (outerScroll)
             outerScroll.OnBeginDrag (eventData);
 
         base.OnBeginDrag (eventData);
     }
 
     public override void OnDrag (PointerEventData eventData)
     {
         if (eventData.button != PointerEventData.InputButton.Left)
             return;
 
         //If this is first frame of drag we need to find out if drag
         //is horizontal or vertical
         if (enteringDrag && outerScroll != null)
         {
             bool horizontalDrag =
                 (Mathf.Abs (eventData.pressPosition.x - eventData.position.x) >
                     Mathf.Abs (eventData.pressPosition.y - eventData.position.y));
 
             //Decide if future draging is to be sent to outer scrollrect
             if (horizontalDrag == outerScroll.horizontal)
                 sendToOuter = true;
 
             //... and send end drag event to the other one.
             if (sendToOuter)
                 base.OnEndDrag (eventData);
             else
                 outerScroll.OnEndDrag (eventData);
         }
         enteringDrag = false;
 
         //Dispatch drag event to the correct scrollrect
         if (sendToOuter)
             outerScroll.OnDrag (eventData);
         else
             base.OnDrag (eventData);
     }
 
     public override void OnEndDrag (PointerEventData eventData)
     {
         if (eventData.button != PointerEventData.InputButton.Left)
             return;
 
         //Dispatch EndDrag event to the correct scrollrect
         if (sendToOuter)
         {
             outerScroll.OnEndDrag (eventData);
             sendToOuter = false;
         }
         else
         {
             base.OnEndDrag (eventData);
         }
     }
 
 }