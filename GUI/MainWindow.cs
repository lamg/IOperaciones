using System;
using Gtk;
using IOperaciones;
using Gdk;

public partial class MainWindow: Gtk.Window
{
	Grafo g;
	int cells, rw, rh, rutaL, rutaW;
	int[] ruta;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		var flt = new Gtk.FileFilter ();
		flt.AddPattern ("*.grafo");
		loadB.Filter = flt;
		aVertice.Adjustment.Upper = 0;
		bVertice.Adjustment.Upper = 0;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnLoadBSelectionChanged (object sender, EventArgs e)
	{
		bool ok;
		g = Recorridos.Cargar (loadB.Filename, out ok);
		if (ok) {
			aVertice.Adjustment.Lower = 0;
			bVertice.Adjustment.Lower = 0;
			aVertice.Adjustment.Upper = g.V - 1;
			bVertice.Adjustment.Upper = g.V - 1;
			int w, h;
			canvas.GdkWindow.GetSize (out w, out h);
			InvalidarCanvas ();
			cells = (int)Math.Sqrt (g.V) + 1;
			rw = w / cells;
			rh = h / cells;
		} else {
			Title = "Error al cargar el archivo";
		}
	}

	protected void OnCanvasExposeEvent (object o, ExposeEventArgs args)
	{
		if (g!=null) {
			for (int i = 0; i < cells; i++) {
				for (int j = 0; j < cells; j++) {
					int u = i * cells + j;
					if (u < g.V) {
						var ly = new Pango.Layout (PangoContext);
						ly.SetMarkup ((i*cells+j).ToString());
						int x0 = j * rw + rw / 2, y0 = i * rh+rh/2;
						//canvas.GdkWindow.DrawRectangle (Style.ForegroundGC (StateType.Selected), true, j * rw, i * rh, rw - 1, rh - 1);
						canvas.GdkWindow.DrawLayout (Style.TextGC(StateType.Normal), x0, y0, ly);

						int c;
						var ady = g.Adyacentes (u, out c);
						for (int k = 0; k < c; k++) {
							int x1, y1;
							ObtCoorCenKRec(ady[k], out x1, out y1);
							canvas.GdkWindow.DrawLine (Style.ForegroundGC (StateType.Normal), x0, y0, x1, y1);
							canvas.GdkWindow.DrawRectangle (Style.ForegroundGC (StateType.Active),true, x1, y1, 4, 4);
							//canvas.GdkWindow.DrawPolygon(Style.ForegroundGC (StateType.Active),true, new Point[]{
							//	new Point(x1, y1), new P
							// esta parte de dibujar las flechas lleva unos calculos
						}
					}
				}
			}

			if (ruta != null) {
				for (int k = 0; k < rutaL-1; k++) {
					int x0, y0, x1, y1;
					ObtCoorCenKRec (ruta [k], out x0, out y0);
					ObtCoorCenKRec (ruta [k + 1], out x1, out y1);
					//Console.WriteLine ("{0}",k+1);
					canvas.GdkWindow.DrawLine (Style.LightGC (StateType.Active), x0, y0, x1, y1);
					// linea entre k y k+1 dibujada
				}
			}
		}
	}

	void ObtCoorCenKRec (int k, out int x, out int y){
		int ik = k / cells, jk = k % cells;
		x = jk * rw + rw / 2;
		y = ik * rh + rh / 2;
	}

	void InvalidarCanvas(){
		int w, h;
		canvas.GdkWindow.GetSize (out w, out h);
		canvas.GdkWindow.InvalidateRect(new Rectangle(0,0,w,h),true);
	}

	Point PointDistAngleB(Point a, Point b, int d, int angle){
		var p = new Point ();
		return p;
	}

	protected void OnRutaDClicked (object sender, EventArgs e)
	{
		if (g != null) {
			ruta = Recorridos.Dijkstra (g, (int)aVertice.Value, (int)bVertice.Value, out rutaW, out rutaL);
			if (rutaL == 0) {
				Title = "No hay camino";
			} else {
				Title = "Peso: " + rutaW.ToString ();
			}
			InvalidarCanvas ();	
		}
	}
}
