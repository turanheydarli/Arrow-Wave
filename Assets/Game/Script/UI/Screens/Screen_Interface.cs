using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Screen_0_Callback {
    void Show();
    void Hide();
}

public interface Screen_1_Callback {
    void Show(Action a1);
    void Hide();
}

public interface Screen_2_Callback {
    void Show(Action a1, Action a2);
    void Hide();
}

public interface Screen_3_Callback {
    void Show(Action a1, Action a2, Action a3);
    void Hide();
}

public interface Screen_4_Callback {
    void Show(Action a1, Action a2, Action a3, Action a4);
    void Hide();
}

public interface Screen_5_Callback {
    void Show(Action a1, Action a2, Action a3, Action a4, Action a5);
    void Hide();
}