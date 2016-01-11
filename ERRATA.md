This document describes the few errata that exist within Flare.

##### Orthographic Camera

If using `CreateOrthographicTopLeftOrigin(float width, float height, float zNear, float zFar)`, keep in mind that this mirrors the y direction so that +y is downwards. This has the side effect of inverting the orientation of any triangles that you draw, (this is depended upon in sprite drawing, by default). This turns counter-clockwise (default) triangles into clockwise ones. This would be an issue if you do anything that is based upon the tri's orientation, including back-face culling and using two-sided materials, but can be accounted for by using glFrontFace to switch the default front-side winding order.